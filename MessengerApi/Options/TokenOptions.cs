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