using MessengerApi.Dtos.Chat;
using MessengerApi.Helpers.Extensions;
using MessengerApi.Repositories.ChatRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MessengerApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class ChatsController : ControllerBase
    {
        private readonly IChatRepository _chatRepository;
        public ChatsController(IChatRepository chatRepository)
        {
            _chatRepository = chatRepository;
        }

        /// <summary>
        /// Gets chats for current user, ordered by last message date.
        /// </summary>

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<SingleChatDto>))]
        public async Task<IActionResult> GetChats([FromQuery] GetChatsDto dto)
        {
            int userId = (int)HttpContext.User.GetId();
            var chats = await _chatRepository
                .GetUserChatsAsync(userId, (int)dto.SkipCount, (int)dto.TakeCount);

            return Ok(chats);
        }

        /// <summary>
        /// Creates new group or private chat.
        /// </summary>
        [HttpPost]
        [ProducesResponseType(201, Type = typeof(int))]
        [ProducesResponseType(400)]
        [ProducesResponseType(409, Type = typeof(int))]
        public async Task<IActionResult> CreateChat([FromBody] CreateChatDto dto)
        {
            int userId = (int)HttpContext.User.GetId();
            
            if (dto.MembersIds.Contains(userId))
                return BadRequest();

            int chatId;
            if (dto.MembersIds.Count >= 2)
                chatId = await _chatRepository.CreateGroupChatAsync(userId, dto.MembersIds);
            else
            {
                (chatId, bool wasCreated) = await _chatRepository.CreatePrivateChatAsync(userId, dto.MembersIds.First());
                if (!wasCreated)
                    return Conflict(chatId);
            }

            return StatusCode(StatusCodes.Status201Created, chatId);
        }
    }
}
