using System.ComponentModel.DataAnnotations;

namespace MessengerApi.Dtos.Auth
{
    public class EmailVerificationDto
    {
        [Required]
        [EmailAddress]
        [MaxLength(100)]
        public string Email { get; set; }

        [Required]
        public string Token { get; set; }
    }
}