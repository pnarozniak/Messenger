using MessengerApi.Database;
using Microsoft.EntityFrameworkCore;

namespace MessengerApi.Repositories.UserRepository
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration;

        public UserRepository(
            AppDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }
        public async Task<User> GetUserByEmailAsync(string email)
        {
            return await _context.Users.SingleOrDefaultAsync(u => u.Email == email);
        }

        public async Task<bool> SetUserRefreshTokenAsync(int idUser, string refreshToken)
        {
            var user = new User()
            {
                Id = idUser,
                RefreshToken = refreshToken,
                RefreshTokenExpiration = DateTime.Now.AddMinutes(int.Parse(_configuration["RefreshToken:ValidityInMinutes"]))
            };

            _context.Entry(user).Property(u => new {u.RefreshToken, u.RefreshTokenExpiration}).IsModified = true;
            return await _context.SaveChangesAsync() > 0;
        }
    }
}