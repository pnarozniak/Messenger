using System.Security.Claims;
using MessengerApi.Database;
using MessengerApi.Database.Models;
using MessengerApi.Helpers;

namespace MessengerApi.Services.ITokenService
{
    public interface ITokenService
    {
        /// <summary>
        /// Generates access token for user
        /// </summary>
        /// <param name="user">User</param>
        /// <returns>Access token as string</returns>
        string GenerateAccessToken(User user);

        /// <summary>
        /// Generates random refresh token
        /// </summary>
        /// <returns>Refresh token</returns>
        RefreshToken GenerateRefreshToken();

        /// <summary>
        /// Generates register confirmation token
        /// </summary>
        /// <returns>Registration token as string</returns>
        string GenerateRegisterConfirmationToken();

        /// <summary>
        /// Validates access token, without its lifetime and parses its claims.
        /// </summary>
        /// <param name="accessToken">Access token as string</param>
        /// <returns>Parsed claims or null if validation fails</returns>
        ClaimsPrincipal GetPrincipalFromExpiredToken(string accessToken);
    }
}