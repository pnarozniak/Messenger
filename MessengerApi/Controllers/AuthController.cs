using MessengerApi.Database;
using MessengerApi.Dtos.Auth;
using MessengerApi.Helpers;
using MessengerApi.Repositories.UserRepository;
using MessengerApi.Services.ITokenService;
using MessengerApi.Services.IEmailSender;
using Microsoft.AspNetCore.Mvc;

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
        
        [HttpPost]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto dto)
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
            var isTokenSet = await _userRepository.SetUserRefreshTokenAsync(
                user.Id, refreshToken);
            if (!isTokenSet)
                return StatusCode(StatusCodes.Status500InternalServerError);

            var response = new LoginResponseDto()
            {
                RefreshToken = refreshToken.Value,
                AccessToken = _tokenService.GenerateAccessToken(user)
            };

            return Ok(response);
        }

        [HttpPost]
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

        [HttpPost]
        public async Task<IActionResult> ConfirmRegister([FromBody] ConfirmRegisterRequestDto dto)
        {
            User user = await _userRepository.GetUserByEmailAsync(dto.Email);
            if (user is null)
                return NotFound();

            if (user.RegisterConfirmationToken != dto.ConfirmationToken)
                return NotFound();

            await _userRepository.MarkUserAsRegisteredAsync(user.Id);
            return NoContent();
        }
    }
}