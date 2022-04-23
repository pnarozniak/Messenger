using MessengerApi.Database.Configurations;
using MessengerApi.Database.Models;
using Microsoft.EntityFrameworkCore;

namespace MessengerApi.Database
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions options)
            :base(options){}
            
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new UserEfConfiguration());
            modelBuilder.ApplyConfiguration(new BlockadeEfConfiguration());
            modelBuilder.ApplyConfiguration(new FriendshipEfConfiguration());
            modelBuilder.ApplyConfiguration(new FriendshipRequestEfConfiguration());
            modelBuilder.ApplyConfiguration(new ChatEfConfiguration());
            modelBuilder.ApplyConfiguration(new UserChatEfConfiguration());
            modelBuilder.ApplyConfiguration(new MessageEfConfiguration());
        }

        public virtual DbSet<User> Users {get; set;}
        public virtual DbSet<Blockade> Blockades {get; set;}
        public virtual DbSet<Friendship> Friendships {get; set;}
        public virtual DbSet<FriendshipRequest> FriendshipRequests {get; set;}
        public virtual DbSet<Chat> Chats {get; set;}
        public virtual DbSet<UserChat> UserChats {get; set;}
        public virtual DbSet<Message> Messages {get; set;}
    }
}