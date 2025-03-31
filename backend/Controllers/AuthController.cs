using backend.DTOs;
using backend.Interfaces;
using backend.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService authService;

        public AuthController(IAuthService authService)
        {
            this.authService=authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> RegisterAsync([FromBody] RegisterDto registerDto)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var user = await authService.RegisterAsync(username: registerDto.UserName, email: registerDto.UserEmail, password: registerDto.Password);
            if(user == null)
            {
                return Conflict("Already Registered! Login To Continue...");
            }
            return Ok(new { Message = "Registration Successful! Login To Continue..." });
        }

        [HttpPost("login")]
        public async Task<IActionResult> LoginAsync([FromBody] LoginDto loginDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = await authService.LoginAsync(email: loginDto.UserEmail ,password: loginDto.Password);
            if (user == null)
            {
                return BadRequest("Invalid Credentials! Try Again..." );
            }
            var token = authService.GenerateToken(user);
            var refreshToken = authService.GenerateRefreshToken(user);
            return Ok(new { Token = token, RefreshToken = refreshToken });
        }

        [HttpPost("refresh")]
        public async Task<IActionResult> RefreshTokenAsync([FromBody] RefreshTokenDto refreshTokenDto)
        {
            var user = await authService.GetUserFromRefreshToken(refreshTokenDto.RefreshToken);
            if (user == null)
            {
                return BadRequest("Login To Continue..." );
            }
            var token = authService.GenerateToken(user);
            var refreshToken = authService.GenerateRefreshToken(user);
            return Ok(new { Token = token, RefreshToken = refreshToken });
        }

        [HttpPost("logout")]
        public async Task<IActionResult> LogoutAsync([FromBody] RefreshTokenDto refreshTokenDto)
        {
            var user = await authService.GetUserFromRefreshToken(refreshTokenDto.RefreshToken);
            if (user == null)
            {
                return BadRequest("You Are Already Logout...");
            }
            await authService.RevokeRefreshTokenAsync(user);
            return Ok(new { Message = "Logged Out Successfully..." });
        }
    }
}
