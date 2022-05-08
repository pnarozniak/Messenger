using MessengerApi.Database.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MessengerApi.Database.Configurations
{
    public class BlockadeEfConfiguration : IEntityTypeConfiguration<Blockade>
    {
        public void Configure(EntityTypeBuilder<Blockade> builder)
        {
            builder.ToTable("Blockade");

            builder.HasKey(b => b.Id)
                .HasName("PK_Blockade");

            builder.Property(b => b.Id)
                .UseMySqlIdentityColumn();

            builder.Property(b => b.CreateDate)
                .HasColumnType("TIMESTAMP")
                .IsRequired();
            
            builder.Property(b => b.IdBlocker)
                .IsRequired();

            builder.Property(b => b.IdBlocked)
                .IsRequired();

            builder.HasOne(b => b.IdBlockerNavigation)
                .WithMany(b => b.CreatedBlockades)
                .HasForeignKey(b => b.IdBlocker)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(b => b.IdBlockedNavigation)
                .WithMany(b => b.ReceivedBlockades)
                .HasForeignKey(b => b.IdBlocked)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}