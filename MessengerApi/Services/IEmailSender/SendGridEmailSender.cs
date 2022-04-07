using MessengerApi.Options;
using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace MessengerApi.Services.IEmailSender
{
    public class SendGridEmailSender : IEmailSender
    {
        public SendGridOptions _options { get; set; }
        public SendGridEmailSender(IOptions<EmailOptions> options)
        {
            _options = options.Value.SendGrid;
        }

        public async Task SendRegisterConfirmationEmailAsync(string email, string confirmationToken)
        {
            var emailMessage = $"<span>Your code is: <b>confirmationToken</b></span>";
            await SendEmailAsync(email, "Finish your registration", emailMessage);
        }

        private async Task SendEmailAsync(string email, string subject, string message)
        {
            var client = new SendGridClient(_options.ApiKey);
            var emailMessage = new SendGridMessage()
            {
                From = new EmailAddress(_options.SenderEmail, _options.SenderName),
                Subject = subject,
                PlainTextContent = message,
                HtmlContent = message
            };
            emailMessage.AddTo(new EmailAddress(email));
 
            emailMessage.SetClickTracking(false, false);
            emailMessage.SetOpenTracking(false);
            emailMessage.SetGoogleAnalytics(false);
            emailMessage.SetSubscriptionTracking(false);

            await client.SendEmailAsync(emailMessage);
        }
    }
}