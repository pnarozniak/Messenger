using MessengerApi.Database;
using MessengerApi.Database.Models;
using MessengerApi.Dtos.Chat;
using Microsoft.EntityFrameworkCore;

namespace MessengerApi.Repositories.ChatRepository
{
    public class ChatRepository : IChatRepository
    {
        private readonly AppDbContext _context;
        public ChatRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<SingleChatDto>> GetUserChatsAsync(int userId, int skipCount, int takeCount)
        {
            return await _context.Chats
                .Where(c => c.UserChats.Any(uc => uc.IdUser == userId))
                .OrderByDescending(c => c.Messages.OrderByDescending(m => m.SendDate).FirstOrDefault().SendDate)
                .Select(c => new SingleChatDto()
                {
                    Id = c.Id,
                    Name = c.Name,
                    IsPrivate = c.IsPrivate,
                    LastMessage = c.Messages.OrderByDescending(m => m.SendDate).FirstOrDefault().Text,
                    LastMessageSendDate = c.Messages.OrderByDescending(m => m.SendDate).FirstOrDefault().SendDate,
                    Members = c.UserChats
                        .Where(uc => uc.IdUser != userId)
                        .Select(uc => new ChatMemberDto()
                        {
                            IdUser = uc.IdUser,
                            FirstName = uc.IdUserNavigation.FirstName,
                            LastName = uc.IdUserNavigation.LastName
                        })
                })
                .Skip(skipCount)
                .Take(takeCount)
                .ToListAsync();
        }

        public async Task<int> CreateGroupChatAsync(int creatorId, List<int> membersIds)
        {   
            membersIds.Add(creatorId);

            var newGroupChat = new Chat()
            {
                Name = null,
                IsPrivate = false,
                UserChats = membersIds.Select(memberId => new UserChat()
                {
                    IdUser = memberId,
                }).ToList(),
            };

            await _context.Chats.AddAsync(newGroupChat);
            await _context.SaveChangesAsync();
            return newGroupChat.Id;
        }

        public async Task<(int chatId, bool wasCreated)> CreatePrivateChatAsync(int creatorId, int memberId)
        {
            var existingPrivateChat = await _context.Chats
                .SingleOrDefaultAsync(c => c.IsPrivate && 
                    c.UserChats.Any(uc => uc.IdUser == creatorId) && 
                    c.UserChats.Any(uc => uc.IdUser == memberId));

            if (existingPrivateChat is not null)
                return (existingPrivateChat.Id, false);

            var newPrivateChat = new Chat()
            {
                Name = null,
                IsPrivate = true,
                UserChats = new List<UserChat>()
                {
                    new() { IdUser = creatorId },
                    new() { IdUser = memberId }
                }
            };
            await _context.Chats.AddAsync(newPrivateChat);
            await _context.SaveChangesAsync();
            return (newPrivateChat.Id, true);
        }
    }
}