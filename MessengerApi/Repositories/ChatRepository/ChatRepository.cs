using MessengerApi.Database;
using MessengerApi.Database.Models;
using MessengerApi.Dtos.Chat;
using MessengerApi.Dtos.Search;
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

        public async Task<IEnumerable<SingleChatDto>> GetUserChatsAsync(int userId, int skipCount, int takeCount, string serachPhraze)
        {   
            var chatMembersToTake = 5;
            serachPhraze = serachPhraze?.ToLower() ?? "";

            var chats = await _context.Chats
                .Where(c => c.UserChats.Any(uc => uc.IdUser == userId))
                .Where(c => 
                    c.Name.ToLower().Contains(serachPhraze) || 
                    c.UserChats
                        .Where(uc => uc.IdUser != userId)
                        .Select(uc => uc.IdUserNavigation.FirstName + " " + uc.IdUserNavigation.LastName)
                        .Any(name => name.ToLower().Contains(serachPhraze)))
                .OrderByDescending(c => c.Messages.OrderByDescending(m => m.SendDate).FirstOrDefault().SendDate)
                .Select(c => new SingleChatDto()
                {
                    Id = c.Id,
                    Name = c.Name,
                    IsPrivate = c.IsPrivate,
                    LastMessage = new LastMessageDto()
                    {
                        Id = c.Messages.OrderByDescending(m => m.SendDate).FirstOrDefault().Id,
                        Text = c.Messages.OrderByDescending(m => m.SendDate).FirstOrDefault().Text,
                        IsRemoved = c.Messages.OrderByDescending(m => m.SendDate).FirstOrDefault().IsRemoved,
                        SendDate = c.Messages.OrderByDescending(m => m.SendDate).FirstOrDefault().SendDate
                    },
                    Members = c.UserChats
                        .Where(uc => uc.IdUser != userId)
                        .Select(uc => new SingleUserDto()
                        {
                            Id = uc.IdUser,
                            FirstName = uc.IdUserNavigation.FirstName,
                            LastName = uc.IdUserNavigation.LastName
                        })
                        .Take(chatMembersToTake)
                })
                .Skip(skipCount)
                .Take(takeCount)
                .ToListAsync();

            chats.ForEach(c => c.LastMessage.Text = c.LastMessage.IsRemoved ? "" : c.LastMessage.Text);
            return chats;
        }

        public async Task<int> CreateGroupChatAsync(int creatorId, List<int> membersIds, string initialMessageText)
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
                Messages = new List<Message>()
                {
                    new Message()
                    {
                        Text = initialMessageText,
                        SendDate = DateTime.Now,
                        IdUser = creatorId,
                    }
                }
            };

            await _context.Chats.AddAsync(newGroupChat);
            await _context.SaveChangesAsync();
            return newGroupChat.Id;
        }

        public async Task<(int chatId, bool wasCreated)> CreatePrivateChatAsync(int creatorId, int memberId, string initialMessageText)
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
                },
                Messages = new List<Message>()
                {
                    new Message()
                    {
                        Text = initialMessageText,
                        SendDate = DateTime.Now,
                        IdUser = creatorId,
                    }
                }
            };
            await _context.Chats.AddAsync(newPrivateChat);
            await _context.SaveChangesAsync();
            return (newPrivateChat.Id, true);
        }

        public async Task<IEnumerable<SingleMessageDto>> GetChatMessagesAsync(int chatId, int userId, GetMessagesRequestDto dto)
        {
            return await _context.Messages
                    .Where(m => m.IdChat == chatId)
                    .Where(m => m.IdChatNavigation.UserChats.Any(uc => uc.IdUser == userId))
                    .OrderByDescending(m => m.SendDate)
                    .Select(m => new SingleMessageDto()
                    {
                        Id = m.Id,
                        Text = m.IsRemoved ? "" : m.Text,
                        SendDate = m.SendDate,
                        IsRemoved = m.IsRemoved,
                        Sender = new SenderDto()
                        {
                            Id = m.IdUser,
                            FirstName = m.IdUserNavigation.FirstName,
                            LastName = m.IdUserNavigation.LastName
                        }
                    })
                    .Skip((int)dto.SkipCount)
                    .Take((int)dto.TakeCount)
                    .ToListAsync();
        }

        public async Task<ChatInfoDto> GetChatInfoAsync(int chatId, int userId)
        {
            return await _context.Chats
                .Where(c => c.Id == chatId)
                .Where(c => c.UserChats.Any(uc => uc.IdUser == userId))
                .Select(c => new ChatInfoDto()
                {
                    Name = c.Name,
                    IsPrivate = c.IsPrivate,
                    Members = c.UserChats
                        .Where(uc => uc.IdUser != userId)
                        .Select(uc => new ChatMemberDto()
                        {
                            Id = uc.IdUser,
                            FirstName = uc.IdUserNavigation.FirstName,
                            LastName = uc.IdUserNavigation.LastName
                        })
                })
                .SingleOrDefaultAsync();
        }

        public async Task<SingleMessageDto> AddMessageAsync(int chatId, int userId, SendMessageDto dto)
        {
            var chatExists = await _context.Chats
                .Where(c => c.Id == chatId)
                .Where(c => c.UserChats.Any(uc => uc.IdUser == userId))
                .AnyAsync();
            
            if (!chatExists)
                return null;

            var newMessage = new Message()
            {
                IdChat = chatId,
                IdUser = userId,
                Text = dto.Text,
                SendDate = DateTime.Now,
            };
            
            await _context.Messages.AddAsync(newMessage);
            return await _context.SaveChangesAsync() > 0 ? new SingleMessageDto(){
                Id = newMessage.Id,
                Text = newMessage.Text,
                SendDate = newMessage.SendDate,
                Sender = await _context.Users
                    .Where(u => u.Id == userId)
                    .Select(u => new SenderDto
                    {
                        Id = u.Id,
                        FirstName = u.FirstName
                    }).SingleOrDefaultAsync()
            } : null;
        }

        public async Task<IEnumerable<int>> GetChatMembersIdsAsync(int chatId)
        {
             return await _context.UserChats
                .Where(uc => uc.IdChat == chatId)
                .Select(uc => uc.IdUser)
                .ToListAsync();
        }

        public async Task<bool> DeleteMessageAsync(int userId, int chatId, int messageId)
        {
            var message = await _context.Messages
                .Where(m => m.Id == messageId)
                .Where(m => m.IdChat == chatId)
                .Where(m => m.IdUser == userId)
                .SingleOrDefaultAsync();

            if (message is null)
                return false;

            message.IsRemoved = true;
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<int?> GetChatIdWithUserAsync(int loggedUserId, int otherUserId)
        {   
            var chat = await _context.Chats
                .Where(c => c.UserChats.Any(uc => uc.IdUser == loggedUserId))
                .Where(c => c.UserChats.Any(uc => uc.IdUser == otherUserId))
                .Where(c => c.IsPrivate)
                .SingleOrDefaultAsync();

            return chat?.Id;
        }
    }
}