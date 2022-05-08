using MessengerApi.Dtos.Chat;
using MessengerApi.Helpers.Extensions;
using MessengerApi.Hubs;
using MessengerApi.Repositories.ChatRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace MessengerApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class ChatsController : ControllerBase
    {
        private readonly IChatRepository _chatRepository;
        private readonly IHubContext<NotificationsHub, INotificationsHub> _notificationsHub;
        
        public ChatsController(
            IChatRepository chatRepository, 
            IHubContext<NotificationsHub, INotificationsHub> notificationsHubContext)
        {
            _chatRepository = chatRepository;
            _notificationsHub = notificationsHubContext;
        }

        /// <summary>
        /// Creates new group or private chat.
        /// </summary>
        [HttpPost]
        [ProducesResponseType(400)]
        [ProducesResponseType(201, Type = typeof(int))]
        [ProducesResponseType(409, Type = typeof(int))]
        public async Task<IActionResult> CreateChat([FromBody] CreateChatDto dto)
        {
            int userId = (int)HttpContext.User.GetId();
            if (dto.MembersIds.Contains(userId))
                return BadRequest();

            int chatId;
            if (dto.MembersIds.Count >= 2) {
                chatId = await _chatRepository
                    .CreateGroupChatAsync(userId, dto.MembersIds, dto.InitialMessageText);
            }
            else
            {
                (chatId, bool notExists) = await _chatRepository
                    .CreatePrivateChatAsync(userId, dto.MembersIds.First(), dto.InitialMessageText);
                if (!notExists)
                    return Conflict(chatId);
            }
            
            var chatMembersIds = (await _chatRepository
                .GetChatMembersIdsAsync(chatId))
                .Select(id => id.ToString());

            var getLastMessageReqDto = new GetMessagesRequestDto(){ SkipCount = 0, TakeCount = 1 };
            var initialMessage = (await _chatRepository
                .GetChatMessagesAsync(chatId, userId, getLastMessageReqDto))
                .FirstOrDefault();

            await _notificationsHub.Clients.Users(chatMembersIds)
                .NewMessageAsync(chatId, initialMessage);

            return StatusCode(StatusCodes.Status201Created, chatId);
        }

        /// <summary>
        /// Gets recent messages for given chat.
        /// </summary>
        [HttpGet("{chatId:int}/messages")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<SingleMessageDto>))]
        public async Task<IActionResult> GetChatMessages([FromRoute] int chatId, [FromQuery] GetMessagesRequestDto dto)
        {
            int userId = (int)HttpContext.User.GetId();
            var messages = await _chatRepository.GetChatMessagesAsync(chatId, userId, dto);
            
            return Ok(messages);
        }

        /// <summary>
        /// Gets chat basic info
        /// </summary>
        [HttpGet("{chatId:int}")]
        [ProducesResponseType(200, Type = typeof(ChatInfoDto))]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetChatInfo([FromRoute] int chatId)
        {
            int userId = (int)HttpContext.User.GetId();
            var chatInfo = await _chatRepository.GetChatInfoAsync(chatId, userId);
            if (chatInfo is null)
                return NotFound();

            return Ok(chatInfo);
        }

        /// <summary>
        /// Adds new message to chat.
        /// </summary>
        [HttpPost("{chatId:int}/messages")]
        [ProducesResponseType(201, Type = typeof(int))]
        [ProducesResponseType(404)]
        public async Task<IActionResult> SendMessage([FromRoute]int chatId, [FromBody]SendMessageDto dto)
        {
            int userId = (int)HttpContext.User.GetId();
            var addedMessage = await _chatRepository.AddMessageAsync(chatId, userId, dto);

            if (addedMessage is null)
                return NotFound();

            var chatMembersIds = (await _chatRepository
                .GetChatMembersIdsAsync(chatId))
                .Where(id => id != userId)
                .Select(id => id.ToString());

            await _notificationsHub.Clients.Users(chatMembersIds)
                .NewMessageAsync(chatId, addedMessage);

            return StatusCode(StatusCodes.Status201Created, addedMessage.Id);
        }

        /// <summary>
        /// Delete message from chat.
        /// </summary>
        [HttpDelete("{chatId:int}/messages/{messageId:int}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> DeleteMessage([FromRoute]int chatId, [FromRoute]int messageId)
        {
            int userId = (int)HttpContext.User.GetId();
            var isDeleted = await _chatRepository.DeleteMessageAsync(userId, chatId, messageId);

            var chatMembersIds = (await _chatRepository
                .GetChatMembersIdsAsync(chatId))
                .Where(id => id != userId)
                .Select(id => id.ToString());

            await _notificationsHub.Clients.Users(chatMembersIds)
                .MessageRemovedAsync(chatId, messageId);

            return isDeleted? NoContent() : NotFound();
        }

        [HttpGet("with-user/{userId:int}")]
        [ProducesResponseType(200, Type = typeof(int))]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetChatIdWithUser([FromRoute]int userId)
        {
            int loggedUserId = (int)HttpContext.User.GetId();
            int? chatId = await _chatRepository.GetChatIdWithUserAsync(loggedUserId, userId);
            
            return chatId.HasValue ? Ok(chatId) : NotFound();
        }
    }
}
