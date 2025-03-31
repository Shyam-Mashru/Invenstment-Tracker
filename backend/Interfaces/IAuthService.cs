

using backend.Models;

namespace backend.Interfaces
{
    public interface IAuthService
    {

        Task<User?> RegisterAsync(string username, string email, string password);
        Task<User?> LoginAsync(string email, string password);
        Task<string> GenerateToken(User user);
        Task<string> GenerateRefreshToken(User user);
        Task<User?> GetUserFromRefreshToken(string refreshToken);
        Task RevokeRefreshTokenAsync(User user);

    }
}
