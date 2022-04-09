using System.ComponentModel.DataAnnotations;

namespace MessengerApi.Dtos.Auth
{
    public class TokensDto
    {
        [Required]
        public string AccessToken { get; set; }

        [Required]
        [MaxLength(36)]
        [MinLength(36)]
        public string RefreshToken { get; set; }
    }
}