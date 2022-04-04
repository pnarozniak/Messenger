using MessengerApi.Ef.Configurations;
using Microsoft.EntityFrameworkCore;

namespace MessengerApi.Ef
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions opt) : base(opt)
        { 

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new UserEfConfiguration());
        }

        public virtual DbSet<User> Users {get; set;}
    }
}