namespace MessengerApi.Dtos.Chat
{
    public class SingleChatDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsPrivate { get; set; }
        public string LastMessage { get; set; }
        public DateTime? LastMessageSendDate { get; set; }
        
        public IEnumerable<ChatMemberDto> Members { get; set; }
    }
}