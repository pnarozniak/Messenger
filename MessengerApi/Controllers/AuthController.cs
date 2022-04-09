using MessengerApi.Database;
using MessengerApi.Dtos.Auth;
using MessengerApi.Helpers;
using MessengerApi.Repositories.UserRepository;
using MessengerApi.Services.ITokenService;
using MessengerApi.Services.IEmailSender;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using MessengerApi.Helpers.Extensions;

namespace MessengerApi.Controllers
{
    [ApiController]
    [Route("/api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly ITokenService _tokenService;
        private readonly IUserRepository _userRepository;
        private readonly IEmailSender _emailSender;

        public AuthController(
            ITokenService tokenService, IUserRepository userRepository,
            IEmailSender emailSender)
        {
            _tokenService = tokenService;
            _userRepository = userRepository;
            _emailSender = emailSender;
        }

        /// <summary>
        /// Login user
        /// </summary>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(TokensDto))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]        
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            User user = await _userRepository.GetUserByEmailAsync(dto.Email);
            if (user is null)
                return Unauthorized();

            var isPasswordMatching = BCrypt.Net.BCrypt.Verify(
                dto.PlainPassword, user.HashedPassword);
            if (!isPasswordMatching)
                return Unauthorized();

            if (user.RegisterConfirmationToken is not null)
                return StatusCode(StatusCodes.Status403Forbidden);

            RefreshToken refreshToken = _tokenService.GenerateRefreshToken();
            await _userRepository.SetUserRefreshTokenAsync(user.Id, refreshToken);

            var response = new TokensDto()
            {
                RefreshToken = refreshToken.Value,
                AccessToken = _tokenService.GenerateAccessToken(user)
            };

            return Ok(response);
        }

        /// <summary>
        /// Register user in db and send register confirmation email
        /// </summary>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> Register([FromBody] RegisterRequestDto dto)
        {
            string confirmationToken = _tokenService.GenerateRegisterConfirmationToken();
            User createdUser = await _userRepository.CreateUserAsync(dto, confirmationToken);
            if (createdUser is null)
                return Conflict();

            await _emailSender.SendRegisterConfirmationEmailAsync(
                createdUser.Email, confirmationToken);
            
            return NoContent();
        }

        /// <summary>
        /// Confirm user registration via token
        /// </summary>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> ConfirmRegister([FromBody] ConfirmRegisterDto dto)
        {
            User user = await _userRepository.GetUserByEmailAsync(dto.Email);
            if (user is null)
                return NotFound();

            if (user.RegisterConfirmationToken != dto.RegisterConfirmationToken)
                return NotFound();

            await _userRepository.MarkUserAsRegisteredAsync(user.Id);
            return NoContent();
        }

        /// <summary>
        /// Refresh user session by generating new access token
        /// </summary>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(TokensDto))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> RefreshToken([FromBody] TokensDto dto)
        {
            ClaimsPrincipal principal = _tokenService.GetPrincipalFromExpiredToken(dto.AccessToken);
            if (principal is null)
                return Unauthorized();

            int? userId = principal.GetId();
            if (userId is null)
                return Unauthorized();

            User user = await _userRepository.GetUserByRefreshTokenAsync(dto.RefreshToken);
            if (user is null || user.Id != userId) 
                return Unauthorized();

            RefreshToken refreshToken = _tokenService.GenerateRefreshToken();
            await _userRepository.SetUserRefreshTokenAsync(user.Id, refreshToken);

            var response = new TokensDto()
            {
                RefreshToken = refreshToken.Value,
                AccessToken = _tokenService.GenerateAccessToken(user)
            };

            return Ok(response);
        }

        /// <summary>
        /// Logout user by removing the refresh token from database
        /// </summary>
        [Authorize]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Logout([FromBody] LogoutDto dto)
        {
            User user = await _userRepository.GetUserByRefreshTokenAsync(dto.RefreshToken);
            if (user is null)
                return Unauthorized();

            int userId = (int)HttpContext.User.GetId();
            if (user.Id != userId) 
                return Unauthorized();
            
            await _userRepository.SetUserRefreshTokenAsync(user.Id, null);
            return NoContent();
        }
    }
}