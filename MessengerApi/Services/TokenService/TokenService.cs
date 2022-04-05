using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using MessengerApi.Database;
using Microsoft.IdentityModel.Tokens;

namespace MessengerApi.Services.TokenService
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _configuration;

        public TokenService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GeneratAccessToken(User user)
        {
            var userClaims = new List<Claim>
            {
                new(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new(ClaimTypes.Email, user.Email.ToString()),
            };

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_configuration["AccessToken:SecretKey"]));

            
            var jwt = new JwtSecurityToken(
                issuer: _configuration["AccessToken:ValidIssuer"],
                audience: _configuration["AccessToken:ValidAudience"],
                claims: userClaims,
                expires: DateTime.UtcNow.AddMinutes(int.Parse(_configuration["AccessToken:ValidityInMinutes"])),
                signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
            );

            return new JwtSecurityTokenHandler().WriteToken(jwt);
        }

        public string GenerateRefreshToken()
        {
            return Guid.NewGuid().ToString();
        }

        public ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = false,
                ValidAudience = _configuration["AccessToken:ValidAudience"],
                ValidateIssuer = false,
                ValidIssuer = _configuration["AccessToken:ValidIssuer"],
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(_configuration["AccessToken:SecretKey"])),
                ValidateLifetime = false
            };

            ClaimsPrincipal principal = null;
            SecurityToken securityToken = null;
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                principal = tokenHandler.ValidateToken(
                    token, tokenValidationParameters, out SecurityToken outSecurityToken);

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