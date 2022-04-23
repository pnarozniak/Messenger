namespace MessengerApi.Database.Models
{
    public class FriendshipRequest
    {
        public int Id { get; set; }
        public DateTime CreateDate { get; set; }

        public int IdSender { get; set; }
        public User IdSenderNavigation { get; set; }
        public int IdReceiver { get; set; }
        public User IdReceiverNavigation { get; set; }
    }
}