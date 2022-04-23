using MessengerApi.Database.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MessengerApi.Database.Configurations
{
    public class MessageEfConfiguration : IEntityTypeConfiguration<Message>
    {
        public void Configure(EntityTypeBuilder<Message> builder)
        {
            builder.ToTable("Message");

            builder.HasKey(m => m.Id)
                .HasName("PK_Message");

            builder.Property(m => m.Id)
                .UseMySqlIdentityColumn();

            builder.Property(m => m.Text)
                .IsRequired();

            builder.Property(m => m.SendDate)
                .IsRequired();
            
            builder.Property(m => m.IdChat)
                .IsRequired();

            builder.Property(m => m.IdUser)
                .IsRequired();

            builder.HasOne(m => m.IdUserNavigation)
                .WithMany(u => u.SentMessages)
                .HasForeignKey(m => m.IdUser)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}