using LTS.API.Domain.Entities;

namespace LTS.API.Infrastructure.Services.JWT
{
    public interface ITokenService
    {
        string GenerateToken(User user);
        string GenerateRefreshToken();
        string HashToken(string token);
    }
}
