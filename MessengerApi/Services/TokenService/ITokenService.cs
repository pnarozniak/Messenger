using System.Security.Claims;
using MessengerApi.Database;

namespace MessengerApi.Services.TokenService
{
    public interface ITokenService
    {
        string GenerateAccessToken(User user);
        string GenerateRefreshToken();
        ClaimsPrincipal GetPrincipalFromExpiredToken(string token);
    }
}