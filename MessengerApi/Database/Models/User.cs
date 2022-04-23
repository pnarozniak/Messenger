namespace MessengerApi.Database.Models
{
    public class User 
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string HashedPassword { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime Birthdate { get; set; }
        public bool IsVerified { get; set; }
        public string? EmailVerificationToken {get; set;}
        public string? RefreshToken { get; set; }
        public DateTime? RefreshTokenExpiration { get; set; }

        public ICollection<Blockade> CreatedBlockades { get; set; }
        public ICollection<Blockade> ReceivedBlockades { get; set; }

        public ICollection<Friendship> Friendships { get; set; }

        public ICollection<FriendshipRequest> SentFriendshipRequests { get; set; }
        public ICollection<FriendshipRequest> ReceivedFriendshipRequests { get; set; }

        public ICollection<Message> SentMessages { get; set; }
        public ICollection<UserChat> UserChats { get; set; }
    }
}