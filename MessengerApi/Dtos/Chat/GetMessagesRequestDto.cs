using System.ComponentModel.DataAnnotations;

namespace MessengerApi.Dtos.Chat
{
    public class GetMessagesRequestDto
    {
        [Required]
        [Range(1, int.MaxValue)]
        public int? TakeCount { get; set; }

        [Required]
        [Range(0, int.MaxValue)]
        public int? SkipCount { get; set; }
    }
}