namespace MessengerApi.Options
{
    public class EmailOptions
    {
        public SendGridOptions SendGrid { get; set; }
    }

    public class SendGridOptions
    {
        public string ApiKey { get; set; }
        public string SenderEmail { get; set; }
        public string SenderName { get; set; }
    }
}