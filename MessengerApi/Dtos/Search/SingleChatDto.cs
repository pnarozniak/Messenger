namespace MessengerApi.Dtos.Search
{
    public class SingleChatDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsPrivate { get; set; }
        public LastMessageDto LastMessage { get; set; }
        public IEnumerable<SingleUserDto> Members { get; set; }
    }

    public class LastMessageDto 
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public bool IsRemoved { get; set; }
        public DateTime SendDate { get; set; }
    }
}