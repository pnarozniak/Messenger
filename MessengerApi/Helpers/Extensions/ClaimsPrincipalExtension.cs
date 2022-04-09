using System.Security.Claims;

namespace MessengerApi.Helpers.Extensions
{
    public static class ClaimsPrincipalExtension
    {
        /// <summary>
        /// Get id from claims principal
        /// </summary>
        /// <param name="principal">Claims principal</param>
        /// <returns>Id if present or null</returns>
        public static int? GetId(this ClaimsPrincipal claimsPrincipal)
        {
            var id = claimsPrincipal.FindFirstValue(ClaimTypes.NameIdentifier);
            var canParse = int.TryParse(id, out var parsedId);
            if (canParse)
                return parsedId;
            return null;
        }
    }
}