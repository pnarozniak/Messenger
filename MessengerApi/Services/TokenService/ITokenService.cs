using System.Security.Claims;
using MessengerApi.Database;

namespace MessengerApi.Services.TokenService
{
    public interface ITokenService
    {
        public string GeneratAccessToken(User user);
        public string GenerateRefreshToken();
        public ClaimsPrincipal GetPrincipalFromExpiredToken(string token);
    }
}