using System.ComponentModel.DataAnnotations;
using MessengerApi.Helpers.ValidationAttributes;

namespace MessengerApi.Dtos.Chat
{
    public class SingleMessageDto
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public DateTime SendDate { get; set; }
        public bool IsRemoved { get; set; }
        public SenderDto Sender { get; set; }
    }

    public class SenderDto 
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}