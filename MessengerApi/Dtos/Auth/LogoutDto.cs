using System.ComponentModel.DataAnnotations;

namespace MessengerApi.Dtos.Auth
{
    public class LogoutDto
    {
        [Required]
        [MinLength(36)]
        [MaxLength(36)]
        public string RefreshToken { get; set; }
    }
}