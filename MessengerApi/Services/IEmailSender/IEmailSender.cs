namespace MessengerApi.Services.IEmailSender
{
    public interface IEmailSender
    {
        Task SendRegisterConfirmationEmailAsync(string email, string confirmationToken);
    }
}