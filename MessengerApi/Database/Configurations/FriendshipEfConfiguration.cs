using MessengerApi.Database.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MessengerApi.Database.Configurations
{
    public class FriendshipEfConfiguration : IEntityTypeConfiguration<Friendship>
    {
        public void Configure(EntityTypeBuilder<Friendship> builder)
        {
            builder.ToTable("Friendship");

            builder.HasKey(f => f.Id)
                .HasName("PK_Friendship");

            builder.Property(f => f.Id)
                .UseMySqlIdentityColumn();

            builder.Property(f => f.CreateDate)
                .HasColumnType("TIMESTAMP")
                .IsRequired();

            builder.Property(f => f.IdRequester)
                .IsRequired();

            builder.Property(f => f.IdAddressee)
                .IsRequired();

            builder.HasOne(f => f.IdRequesterNavigation)
                .WithMany(u => u.Friendships)
                .HasForeignKey(f => f.IdRequester)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(f => f.IdAddresseeNavigation)
                .WithMany(u => u.Friendships)
                .HasForeignKey(f => f.IdAddressee)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}