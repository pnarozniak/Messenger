using System.ComponentModel.DataAnnotations;
using MessengerApi.Helpers.ValidationAttributes;

namespace MessengerApi.Dtos.Chat
{
    public class CreateChatDto
    {
        [CollectionNotNullOrEmptyValidation]
        public List<int> MembersIds { get; set; }
        
        [Required]
        public string InitialMessageText { get; set; }
    }
}