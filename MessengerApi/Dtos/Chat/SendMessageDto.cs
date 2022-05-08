using System.ComponentModel.DataAnnotations;

namespace MessengerApi.Dtos.Chat
{
    public class SendMessageDto
    {
        [Required]
        public string Text { get; set; }   
    }
}