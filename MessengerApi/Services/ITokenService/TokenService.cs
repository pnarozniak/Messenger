using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using MessengerApi.Database;
using MessengerApi.Helpers;
using MessengerApi.Options;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace MessengerApi.Services.ITokenService
{
    public class TokenService : ITokenService
    {
        private readonly TokenOptions _options;

        public TokenService(IOptions<TokenOptions> options)
        {
            _options = options.Value;
        }

        public string GenerateAccessToken(User user)
        {
            var userClaims = new List<Claim>
            {
                new(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new(ClaimTypes.Email, user.Email.ToString()),
            };

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_options.AccessToken.SecretKey));

            
            var jwt = new JwtSecurityToken(
                issuer: _options.AccessToken.ValidIssuer,
                audience: _options.AccessToken.ValidAudience,
                claims: userClaims,
                expires: DateTime.UtcNow.AddMinutes(_options.AccessToken.ValidityInMinutes),
                signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
            );

            return new JwtSecurityTokenHandler().WriteToken(jwt);
        }

        public RefreshToken GenerateRefreshToken()
        {
            return new RefreshToken()
            {
                Value = Guid.NewGuid().ToString(),
                ExpirationDate = DateTime.UtcNow.AddMinutes(_options.RefreshToken.ValidityInMinutes)
            };
        }

        public string GenerateRegisterConfirmationToken()
        {  
            var tokenLength = _options.RegisterConfirmationToken.Length;

            var token = string.Empty;
            Random rnd = new Random();
            foreach(var i in Enumerable.Range(0, tokenLength))
            {
                token += rnd.NextInt64(1, 10);
            }

            return token;
        }

        public ClaimsPrincipal GetPrincipalFromExpiredToken(string accessToken)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = false,
                ValidAudience = _options.AccessToken.ValidAudience,
                ValidateIssuer = false,
                ValidIssuer = _options.AccessToken.ValidIssuer,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(_options.AccessToken.SecretKey)),
                ValidateLifetime = false
            };

            ClaimsPrincipal principal = null;
            SecurityToken securityToken = null;
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                principal = tokenHandler.ValidateToken(
                    accessToken, tokenValidationParameters, out SecurityToken outSecurityToken);

                securityToken = outSecurityToken;
            }
            catch
            {
                return null;
            }
        
            if (securityToken is not JwtSecurityToken jwt || jwt == null 
                || !jwt.Header.Alg.Equals(SecurityAlgorithms.HmacSha256,StringComparison.InvariantCultureIgnoreCase))
                return null;

            return principal;
        }
    }
}