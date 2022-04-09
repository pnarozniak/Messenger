namespace MessengerApi.Services.IEmailSender
{
    public interface IEmailSender
    {
        /// <summary>
        /// Sends register confirmation email directly to user. Email contains confirmation token.
        /// </summary>
        /// <param name="email">User email address</param>
        /// <param name="confirmationToken">Reigser confirmation token</param>
        Task SendRegisterConfirmationEmailAsync(string email, string confirmationToken);
    }
}