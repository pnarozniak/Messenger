using System.ComponentModel.DataAnnotations;

namespace MessengerApi.Dtos.Auth
{
    public class LoginRequestDto
    {
        [Required]
        [EmailAddress]
        [MaxLength(100)]
        public string Email { get; set; }

        [Required]
        [MaxLength(30)]
        public string PlainPassword { get; set; }
    }

    public class LoginResponseDto 
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
    }
}