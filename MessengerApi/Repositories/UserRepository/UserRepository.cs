using MessengerApi.Database;
using MessengerApi.Dtos.Auth;
using MessengerApi.Helpers;
using Microsoft.EntityFrameworkCore;

namespace MessengerApi.Repositories.UserRepository
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;

        public UserRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<User> CreateUserAsync(RegisterDto registerDto, string registerConfirmationToken)
        {
            var user = new User()
            {
                FirstName = registerDto.FirstName,
                LastName = registerDto.LastName, 
                Birthdate = (DateTime)registerDto.Birthdate,
                Email = registerDto.Email, 
                RegisterConfirmationToken = registerConfirmationToken,
                HashedPassword = BCrypt.Net.BCrypt.HashPassword(registerDto.PlainPassword)
            };
            
            try {
                await _context.Users.AddAsync(user);
                return await _context.SaveChangesAsync() > 0 ? user : null;            }
            catch (DbUpdateException) {
                return null;
            }
        }

        public async Task<User> GetUserByEmailAsync(string email)
        {
            return await _context.Users.SingleOrDefaultAsync(u => u.Email == email);
        }

        public async Task SetUserRefreshTokenAsync(int idUser, RefreshToken refreshToken = null)
        {
            var user = new User()
            {
                Id = idUser,
                RefreshToken = refreshToken?.Value,
                RefreshTokenExpiration = refreshToken?.ExpirationDate
            };

            _context.Entry(user).Property(u => new {u.RefreshToken, u.RefreshTokenExpiration}).IsModified = true;
            await _context.SaveChangesAsync();
        }

        public async Task MarkUserAsRegisteredAsync(int idUser)
        {
            var user = new User()
            {
                Id = idUser,
                RegisterConfirmationToken = null,
            }; 

            _context.Entry(user).Property(u => u.RegisterConfirmationToken).IsModified = true;
            await _context.SaveChangesAsync();
        }

        public Task<User> GetUserByRefreshTokenAsync(string refreshToken)
        {
            return _context.Users.SingleOrDefaultAsync(u => u.RefreshToken == refreshToken);
        }
    }
}