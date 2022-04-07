using MessengerApi.Database;
using MessengerApi.Dtos.Auth;
using MessengerApi.Helpers;

namespace MessengerApi.Repositories.UserRepository
{
    public interface IUserRepository
    {
        Task<User> GetUserByEmailAsync(string email);
        Task<bool> SetUserRefreshTokenAsync(int idUser, RefreshToken refreshToken);
        Task<User> CreateUserAsync(RegisterRequestDto registerDto, string registerConfirmationToken);
        Task MarkUserAsRegisteredAsync(int idUser);
    }
}