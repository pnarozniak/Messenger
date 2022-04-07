using MessengerApi.Database.Configurations;
using MessengerApi.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace MessengerApi.Database
{
    public class AppDbContext : DbContext
    {
        private readonly MySqlOptions _options;
        public AppDbContext(IOptions<MySqlOptions> options)
        {
            _options = options.Value;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySql(
                _options.ConnectionString, 
                new MySqlServerVersion(new Version(_options.ServerVersion))
            );
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new UserEfConfiguration());
        }

        public virtual DbSet<User> Users {get; set;}
    }
}