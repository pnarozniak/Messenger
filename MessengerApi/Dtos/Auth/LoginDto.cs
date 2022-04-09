using System.ComponentModel.DataAnnotations;

namespace MessengerApi.Dtos.Auth
{
    public class LoginDto
    {
        [Required]
        [EmailAddress]
        [MaxLength(100)]
        public string Email { get; set; }

        [Required]
        [MaxLength(30)]
        public string PlainPassword { get; set; }
    }
}