using System.ComponentModel.DataAnnotations;

namespace MessengerApi.Dtos.Auth
{
    public class ConfirmRegisterRequestDto
    {
        [Required]
        [EmailAddress]
        [MaxLength(100)]
        public string Email { get; set; }

        [Required]
        public string ConfirmationToken { get; set; }
    }
}