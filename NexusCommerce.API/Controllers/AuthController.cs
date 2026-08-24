using Microsoft.AspNetCore.Mvc;
using NexusCommerce.BLL.DTOs.User;
using NexusCommerce.BLL.Managers.Auth;

namespace NexusCommerce.API.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthManager _authManager;

        public AuthController(IAuthManager authManager)
        {
            _authManager = authManager;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
        {
            var result = await _authManager.RegisterAsync(registerDto);
            if (!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            var result = await _authManager.LoginAsync(loginDto);
            if (!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }
    }
}
