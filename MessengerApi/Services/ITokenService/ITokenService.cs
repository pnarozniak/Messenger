using System.Security.Claims;
using MessengerApi.Database;
using MessengerApi.Helpers;

namespace MessengerApi.Services.ITokenService
{
    public interface ITokenService
    {
        string GenerateAccessToken(User user);
        RefreshToken GenerateRefreshToken();
        string GenerateRegisterConfirmationToken();
        ClaimsPrincipal GetPrincipalFromExpiredToken(string token);
    }
}