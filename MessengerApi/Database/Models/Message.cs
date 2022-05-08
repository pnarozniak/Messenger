namespace MessengerApi.Database.Models
{
    public class Message
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public DateTime SendDate { get; set; }
        public bool IsRemoved { get; set; }

        public int IdUser { get; set; }
        public User IdUserNavigation { get; set; }

        public int IdChat { get; set; }
        public Chat IdChatNavigation { get; set; }
    }
}