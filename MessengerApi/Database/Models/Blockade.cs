namespace MessengerApi.Database.Models
{
    public class Blockade
    {
        public int Id { get; set; }
        public DateTime CreateDate { get; set; }

        public int IdBlocker { get; set; }
        public User IdBlockerNavigation { get; set; }
        public int IdBlocked { get; set; }
        public User IdBlockedNavigation { get; set; }
    }
}