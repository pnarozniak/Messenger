namespace MessengerApi.Options
{
    public class MySqlOptions
    {
        public string Server { get; set; }
        public string User { get; set; }
        public string Password { get; set; }
        public string Database { get; set; }
        public string ServerVersion { get; set; }

        public string ConnectionString {get => $"server={Server};user={User};password={Password};database={Database}"; }
    }
}