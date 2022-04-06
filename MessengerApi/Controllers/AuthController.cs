using System.Net;
using MessengerApi.Database;
using MessengerApi.Dtos.Auth;
using MessengerApi.Repositories.UserRepository;
using MessengerApi.Services.TokenService;
using Microsoft.AspNetCore.Mvc;

namespace MessengerApi.Controllers
{
    [ApiController]
    [Route("/api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly ITokenService _tokenService;
        private readonly IUserRepository _userRepository;

        public AuthController(
            ITokenService tokenService, IUserRepository userRepository)
        {
            _tokenService = tokenService;
            _userRepository = userRepository;
        }
        
        [HttpPost]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto dto)
        {
            User user = await _userRepository.GetUserByEmailAsync(dto.Email);
            if (user is null)
                return BadRequest();

            var isPasswordMatching = BCrypt.Net.BCrypt.Verify(
                dto.PlainPassword, user.HashedPassword);
            if (!isPasswordMatching)
                return Unauthorized();

            var response = new LoginResponseDto()
            {
                RefreshToken = _tokenService.GenerateRefreshToken(),
                AccessToken = _tokenService.GenerateAccessToken(user)
            };

            var isTokenSet = await _userRepository.SetUserRefreshTokenAsync(
                user.Id, response.RefreshToken);
            if (!isTokenSet)
                return StatusCode(StatusCodes.Status500InternalServerError);

            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Register()
        {
            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> Refresh()
        {
            return Ok();
        }
    }
}