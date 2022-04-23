using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace MessengerApi.Options
{
    public class TokenOptions
    {
        public AccessTokenOptions AccessToken { get; set; }
        public RefreshTokenOptions RefreshToken { get; set; }
        public RegisterConfirmationTokenOptions RegisterConfirmationToken {get; set;}
    }

    public class AccessTokenOptions
    {
        public string SecretKey { get; set; }
        public string ValidIssuer { get; set; }
        public string ValidAudience { get; set; }
        public int ValidityInMinutes { get; set; }

        public TokenValidationParameters ValidationParameters => 
            new()
            {
                ValidateIssuer = true,
                ValidIssuer = ValidIssuer,
                ValidateAudience = true,
                ValidAudience = ValidAudience,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(SecretKey)),
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero
            };

        public Task HandleAuthenticationFailed(AuthenticationFailedContext ctx)
        {
            if (ctx?.Exception is not null && ctx.Exception.GetType() == typeof(SecurityTokenExpiredException))
            {
                ctx.Response.Headers.Add("Token-Expired", "true");
            }

            return Task.CompletedTask;
        }
    }

    public class RefreshTokenOptions
    {
        public int ValidityInMinutes { get; set; }
    }

    public class RegisterConfirmationTokenOptions
    {
        public int Length { get; set; }
    }
}