using System.ComponentModel.DataAnnotations;

namespace MessengerApi.Dtos.Auth
{
    public class ConfirmRegisterDto
    {
        [Required]
        [EmailAddress]
        [MaxLength(100)]
        public string Email { get; set; }

        [Required]
        public string RegisterConfirmationToken { get; set; }
    }
}