namespace MessengerApi.Database.Models
{
    public class Friendship
    {
        public int Id { get; set; }
        public DateTime CreateDate { get; set; }

        public int IdRequester { get; set; }
        public User IdRequesterNavigation { get; set; }
        public int IdAddressee { get; set; }
        public User IdAddresseeNavigation { get; set; }
    }
}