namespace MessengerApi.Database.Models
{
    public class UserChat
    {
        public int Id { get; set; }
        
        public int IdUser { get; set; }
        public User IdUserNavigation { get; set; }
        public int IdChat { get; set; }
        public Chat IdChatNavigation { get; set; }
    }
}