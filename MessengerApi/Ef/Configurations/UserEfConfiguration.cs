using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MessengerApi.Ef.Configurations
{
    public class UserEfConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("User");

            builder.HasKey(user => user.Id)
                .HasName("User_pk");

            builder.Property(user => user.Id)
                .UseMySqlIdentityColumn();

            builder.Property(user => user.Email)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(user => user.FirstName)
                .IsRequired()
                .HasMaxLength(32);

            builder.Property(user => user.LastName)
                .IsRequired()
                .HasMaxLength(32);

            builder.Property(user => user.Birthdate)
                .IsRequired()
                .HasColumnType("date");
        }
    }
}