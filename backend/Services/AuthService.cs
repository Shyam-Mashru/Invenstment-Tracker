using backend.Data;
using backend.Interfaces;
using backend.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace backend.Services
{
    public class AuthService : IAuthService
    {
        private readonly BackendDbContext context;

        public AuthService(BackendDbContext context)
        {
            this.context = context;
        }

        public async Task<User?> LoginAsync(string email, string password)
        {
            var user = context.Users.FirstOrDefault(u => u.UserEmail == email);
            if (user == null || !BCrypt.Net.BCrypt.Verify(password, user.Password))
            {
                return null;
            }

            return user;
        }

        public async Task<User?> RegisterAsync(string username, string email, string password)
        {
            var user = context.Users.FirstOrDefault(u => u.UserEmail == email);
            if (user != null)
            {
                return null;
            }

            var newUser = new User
            {
                UserName = username,
                UserEmail = email,
                Password = BCrypt.Net.BCrypt.HashPassword(password),
                CreatedAt = DateTime.Now
            };

            await context.Users.AddAsync(newUser);
            await context.SaveChangesAsync();

            return newUser;
        }

        public async Task<string> GenerateToken(User user)
        {
            var issuer = Environment.GetEnvironmentVariable("INVESTMENT_PROJECT") ?? throw new ArgumentNullException("Issuer is not set.");
            var audience = Environment.GetEnvironmentVariable("INVESTMENT_PROJECT") ?? throw new ArgumentNullException("Audience is not set.");
            var secretKey = Environment.GetEnvironmentVariable("JWT_SECRET_KEY") ?? throw new ArgumentNullException("JWT_SECRET_KEY is not set.");
            var key = Encoding.UTF8.GetBytes(secretKey);

            var claims = new[]
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Email, user.UserEmail),
                new Claim(ClaimTypes.Role, user.Role)
            };

            var creds = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: DateTime.Now.AddMinutes(15),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task<string> GenerateRefreshToken(User user)
        {
            var refreshToken = Guid.NewGuid().ToString();
            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiry = DateTime.Now.AddDays(7);
            context.Users.Update(user);
            await context.SaveChangesAsync();
            return refreshToken;
        }

        public async Task<User?> GetUserFromRefreshToken(string refreshToken)
        {
            return await context.Users.SingleOrDefaultAsync(u => u.RefreshToken == refreshToken);
        }

        public async Task RevokeRefreshTokenAsync(User user)
        {
            user.RefreshToken = string.Empty;
            user.RefreshTokenExpiry = DateTime.MinValue;
            context.Users.Update(user);
            await context.SaveChangesAsync();
        }
    }
}
