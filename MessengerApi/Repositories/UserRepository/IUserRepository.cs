using MessengerApi.Database;

namespace MessengerApi.Repositories.UserRepository
{
    public interface IUserRepository
    {
        Task<User> GetUserByEmailAsync(string email);
        Task<bool> SetUserRefreshTokenAsync(int idUser, string refreshToken);
    }
}