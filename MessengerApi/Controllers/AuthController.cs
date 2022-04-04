using MessengerApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace MessengerApi.Controllers
{
    [ApiController]
    [Route("/api/[controller]")]
    public class AuthController : ControllerBase
    {
        public AuthController()
        {
  
        }
        
        [HttpPost]
        public async Task<IActionResult> Login()
        {
            return Ok();
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