using MessengerApi.Dtos.Chat;
using MessengerApi.Dtos.Search;

namespace MessengerApi.Repositories.ChatRepository
{
    public interface IChatRepository
    {
        /// <summary>
        /// Gets chats for given user, ordered by last message date.
        /// </summary>
        /// <param name="userId">Id of user.</param>
        /// <param name="skipCount">Amount of chats to skip</param>
        /// <param name="chatsCount">Amount of chats to take</param>
        /// <param name="searchPhraze">Phraze to search for in chat name</param>
        /// <returns>List of chats.</returns>
        Task<IEnumerable<SingleChatDto>> GetUserChatsAsync(int userId, int skipCount, int takeCount, string serachPhraze);

        /// <summary>
        /// Creates group chat for given members.
        /// </summary>
        /// <param name="creatorId">Id of user who created chat.</param>
        /// <param name="membersIds">Ids of members.</param>
        /// <param name="initialMessageText">Initial chat message text.</param>
        /// <returns>Id of created group chat.</returns>
        Task<int> CreateGroupChatAsync(int creatorId, List<int> membersIds, string initialMessageText);

        /// <summary>
        /// Creates private chat for given members.
        /// </summary>
        /// <param name="creatorId">Id of user who created chat.</param>
        /// <param name="memberId">Id of member.</param>
        /// <param name="initialMessageText">Initial chat message text.</param>
        /// <returns>Id of created private chat, information if chat was created.</returns>
        Task<(int chatId, bool wasCreated)> CreatePrivateChatAsync(int creatorId, int memberId, string initialMessageText);

        /// <summary>
        /// Gets messages for given chat.
        /// </summary>
        /// <param name="chatId">Id of chat.</param>
        /// <param name="userId">Id of user requesting data.</param>
        /// <param name="dto">Request data transfer object</param>
        /// <returns>List of messages or empty list if user is not a member of given chat</returns>
        Task<IEnumerable<SingleMessageDto>> GetChatMessagesAsync(int chatId, int userId, GetMessagesRequestDto dto);
        
        /// <summary>
        /// Gets chat basic info for given chat.
        /// </summary>
        /// <param name="chatId">Id of chat.</param>
        /// <param name="userId">Id of user requesting data.</param>
        /// <returns>Chat info or null if user is not a member of given chat</returns>
        Task<ChatInfoDto> GetChatInfoAsync(int chatId, int userId);

        /// <summary>
        /// Adds message to given chat.
        /// </summary>
        /// <param name="chatId">Id of chat.</param>
        /// <param name="userId">Id of user who sent message.</param>
        /// <param name="dto">Request data transfer object.</param>
        /// <returns>Created message or null if user is not a member of given chat</returns>
        Task<SingleMessageDto> AddMessageAsync(int chatId, int userId, SendMessageDto dto);

        /// <summary>
        /// Gets chat members ids for given chat.
        /// </summary>
        /// <param name="chatId">Id of chat.</param>
        /// <returns>List of chat members ids or empty list if chat does not exist</returns>
        Task<IEnumerable<int>> GetChatMembersIdsAsync(int chatId);

        /// <summary>
        /// Deletes message from given chat.
        /// </summary>
        /// <param name="userId">Id of user who deleted message.</param>
        /// <param name="chatId">Id of chat.</param>
        /// <param name="messageId">Id of message.</param>
        /// <returns>True if message was deleted or false if message does not exist or user can't delete it</returns>
        Task<bool> DeleteMessageAsync(int userId, int chatId, int messageId);

        /// <summary>
        /// Gets private chat id for given users.
        /// </summary>
        /// <param name="loggedUserId">Id of user requesting data.</param>
        /// <param name="otherUserId">Id of other user.</param>
        Task<int?> GetChatIdWithUserAsync(int loggedUserId, int otherUserId);
    }
}