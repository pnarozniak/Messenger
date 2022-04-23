using MessengerApi.Database.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MessengerApi.Database.Configurations
{
    public class ChatEfConfiguration : IEntityTypeConfiguration<Chat>
    {
        public void Configure(EntityTypeBuilder<Chat> builder)
        {
            builder.ToTable("Chat");

            builder.HasKey(c => c.Id)
                .HasName("PK_Chat");

            builder.Property(c => c.Id)
                .UseMySqlIdentityColumn();

            builder.Property(c => c.Name)
                .HasColumnType("VARCHAR(32)")
                .IsRequired(false);

            builder.Property(c => c.IsPrivate)
                .IsRequired();

            builder.HasMany(c => c.Messages)
                .WithOne(m => m.IdChatNavigation)
                .HasForeignKey(m => m.IdChat)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}