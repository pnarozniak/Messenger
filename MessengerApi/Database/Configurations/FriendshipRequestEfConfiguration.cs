using MessengerApi.Database.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MessengerApi.Database.Configurations
{
    public class FriendshipRequestEfConfiguration : IEntityTypeConfiguration<FriendshipRequest>
    {
        public void Configure(EntityTypeBuilder<FriendshipRequest> builder)
        {
            builder.ToTable("Friendship_Request");

            builder.HasKey(f => f.Id)
                .HasName("PK_FriendshipRequest");

            builder.Property(f => f.Id)
                .UseMySqlIdentityColumn();

            builder.Property(f => f.CreateDate)
                .HasColumnType("TIMESTAMP")
                .IsRequired();

            builder.Property(f => f.IdSender)
                .IsRequired();

            builder.Property(f => f.IdReceiver)
                .IsRequired();

            builder.HasOne(f => f.IdSenderNavigation)
                .WithMany(u => u.SentFriendshipRequests)
                .HasForeignKey(f => f.IdSender)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(f => f.IdReceiverNavigation)
                .WithMany(u => u.ReceivedFriendshipRequests)
                .HasForeignKey(f => f.IdReceiver)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}