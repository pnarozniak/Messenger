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

            builder.Property(f => f.IdUser1)
                .IsRequired();

            builder.Property(f => f.IdUser2)
                .IsRequired();

            builder.HasOne(f => f.IdUser1Navigation)
                .WithMany(u => u.FriendshipsWhereIsUser1)
                .HasForeignKey(f => f.IdUser1)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(f => f.IdUser2Navigation)
                .WithMany(u => u.FriendshipsWhereIsUser2)
                .HasForeignKey(f => f.IdUser2)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}