using MessengerApi.Dtos.Chat;

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
        /// <returns>List of chats.</returns>
        Task<IEnumerable<SingleChatDto>> GetUserChatsAsync(int userId, int skipCount, int takeCount);

        /// <summary>
        /// Creates group chat for given members.
        /// </summary>
        /// <param name="creatorId">Id of user who created chat.</param>
        /// <param name="membersIds">Ids of members.</param>
        /// <returns>Id of created group chat.</returns>
        Task<int> CreateGroupChatAsync(int creatorId, List<int> membersIds);

        /// <summary>
        /// Creates private chat for given members.
        /// </summary>
        /// <param name="creatorId">Id of user who created chat.</param>
        /// <param name="memberId">Id of member.</param>
        /// <returns>Id of created private chat or Id of existing private chat between given users.</returns>
        Task<(int chatId, bool wasCreated)> CreatePrivateChatAsync(int creatorId, int memberId);
    }
}