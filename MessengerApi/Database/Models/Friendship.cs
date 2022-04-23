namespace MessengerApi.Database.Models
{
    public class Friendship
    {
        public int Id { get; set; }
        public DateTime CreateDate { get; set; }

        public int IdUser1 { get; set; }
        public User IdUser1Navigation { get; set; }
        public int IdUser2 { get; set; }
        public User IdUser2Navigation { get; set; }
    }
}