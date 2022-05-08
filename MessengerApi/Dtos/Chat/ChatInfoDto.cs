namespace MessengerApi.Dtos.Chat
{
    public class ChatInfoDto
    {
        public string Name { get; set; }
        public bool IsPrivate { get; set; }
        public IEnumerable<ChatMemberDto> Members { get; set; }
    }

    public class ChatMemberDto
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}