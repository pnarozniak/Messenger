using MessengerApi.Dtos.Search;
using MessengerApi.Helpers.Extensions;
using MessengerApi.Repositories.ChatRepository;
using MessengerApi.Repositories.UserRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MessengerApi.Controllers 
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class SearchController : ControllerBase
    {
        private readonly IChatRepository _chatRepository;
        private readonly IUserRepository _userRepository;

        public SearchController(
            IChatRepository chatRepository,
            IUserRepository userRepository)
        {
            _chatRepository = chatRepository;
            _userRepository = userRepository;
        }

        /// <summary>
        /// Searches for chats in which logged user is a member, ordered by last message date.
        /// </summary>
        [HttpGet("chats")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<SingleChatDto>))]
        public async Task<IActionResult> GetChats([FromQuery] SearchRequestDto dto)
        {
            int userId = (int)HttpContext.User.GetId();
            var chats = await _chatRepository
                .GetUserChatsAsync(userId, (int)dto.SkipCount, (int)dto.TakeCount, dto.SearchPhraze);

            return Ok(chats);
        }

        /// <summary>
        /// Searches for users ordered alphabetically by first and last names.
        /// </summary>
        [HttpGet("more-people")]
        [ProducesResponseType(400)]
        [ProducesResponseType(200, Type = typeof(IEnumerable<SingleUserDto>))]
        public async Task<IActionResult> GetMorePeople([FromQuery] SearchRequestDto dto)
        {
            if (dto.SearchPhraze is null || dto.SearchPhraze.Length < 3)
                return BadRequest("Search phrase must be at least 3 characters long.");

            int userId = (int)HttpContext.User.GetId();
            var users = await _userRepository
                .GetUsersForUserAsync(userId, (int)dto.SkipCount, (int)dto.TakeCount, dto.SearchPhraze);

            return Ok(users);
        }
    }
}