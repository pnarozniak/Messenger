using System.ComponentModel.DataAnnotations;

namespace MessengerApi.Dtos.Chat
{
    public class GetChatsDto
    {
        [Required]
        [Range(0, int.MaxValue)]
        public int? SkipCount { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        public int? TakeCount { get; set; }
    }
}