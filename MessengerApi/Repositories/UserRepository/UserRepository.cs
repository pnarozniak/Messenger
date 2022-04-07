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

        public async Task<User> CreateUserAsync(RegisterRequestDto registerDto, string registerConfirmationToken)
        {
            var user = new User()
            {
                FirstName = registerDto.FirstName,
                LastName = registerDto.LastName, 
                Birthdate = registerDto.Birthdate,
                Email = registerDto.Email, 
                RegisterConfirmationToken = registerConfirmationToken,
                HashedPassword = BCrypt.Net.BCrypt.HashPassword(registerDto.PlainPassword)
            };

            await _context.Users.AddAsync(user);
            return await _context.SaveChangesAsync() > 0 ? user : null;
        }

        public async Task<User> GetUserByEmailAsync(string email)
        {
            return await _context.Users.SingleOrDefaultAsync(u => u.Email == email);
        }

        public async Task<bool> SetUserRefreshTokenAsync(int idUser, RefreshToken refreshToken)
        {
            var user = new User()
            {
                Id = idUser,
                RefreshToken = refreshToken.Value,
                RefreshTokenExpiration = refreshToken.ExpirationDate
            };

            _context.Entry(user).Property(u => new {u.RefreshToken, u.RefreshTokenExpiration}).IsModified = true;
            return await _context.SaveChangesAsync() > 0;
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
    }
}