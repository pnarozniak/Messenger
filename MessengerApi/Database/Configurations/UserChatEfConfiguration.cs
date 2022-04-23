using MessengerApi.Database.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MessengerApi.Database.Configurations
{
    public class UserChatEfConfiguration : IEntityTypeConfiguration<UserChat>
    {
        public void Configure(EntityTypeBuilder<UserChat> builder)
        {
            builder.ToTable("User_Chat");

            builder.HasKey(uc => uc.Id)
                .HasName("PK_User_Chat");

            builder.Property(uc => uc.Id)
                .UseMySqlIdentityColumn();

            builder.Property(uc => uc.IdUser)
                .IsRequired();

            builder.Property(uc => uc.IdChat)
                .IsRequired();

            builder.HasOne(uc => uc.IdUserNavigation)
                .WithMany(u => u.UserChats)
                .HasForeignKey(uc => uc.IdUser)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(uc => uc.IdChatNavigation)
                .WithMany(c => c.UserChats)
                .HasForeignKey(uc => uc.IdChat)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}