using MessengerApi.Database;
using MessengerApi.Database.Models;
using MessengerApi.Dtos.Auth;
using MessengerApi.Helpers;

namespace MessengerApi.Repositories.UserRepository
{
    public interface IUserRepository
    {
        /// <summary>
        /// Gets user by email
        /// </summary>
        /// <param name="email">User email</param>
        /// <returns>User or null if not found</returns>
        Task<User> GetUserByEmailAsync(string email);

        /// <summary>
        /// Sets user refresh token to given value
        /// </summary>
        /// <param name="refreshToken">RefreshToken</param>
        Task SetUserRefreshTokenAsync(int idUser, RefreshToken refreshToken = null);

        /// <summary>
        /// Sets user email verification token to given value
        /// </summary>
        /// <param name="emailVerificaionToken">Email verificaion token</param>
        Task SetUserEmailVerificationTokenAsync(int idUser, string emailVerificaionToken);

        /// <summary>
        /// Creates new user
        /// </summary>
        /// <param name="registerDto">Register data transfer object</param>
        /// <param name="registerConfirmationToken">Generated reigster confirmation token</param>
        /// <returns>Created user or null if not created</returns>
        Task<User> CreateUserAsync(RegisterDto registerDto);

        /// <summary>
        /// Marks user as verified
        /// </summary>
        /// <param name="idUser">User id</param>
        Task MarkUserAsVerifiedAsync(int idUser);

        /// <summary>
        /// Gets user by refresh token
        /// </summary>
        /// <param name="refreshToken">Refresh token</param>
        /// <returns>User or null if not found</returns>
        Task<User> GetUserByRefreshTokenAsync(string refreshToken);
    }
}