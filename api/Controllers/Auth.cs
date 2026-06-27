using api.Helpers;
using api.Model.Dtos.Auth;
using api.Model.Entities;
using api.Service.Auth;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController(IAuthService authService) : ControllerBase
    {
        private readonly IAuthService _authService = authService;

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto data)
        {
            var result = await _authService.LoginAsync(data);
            return StatusCode((int)result.StatusCode, new ApiResponse<object>
            {
                Data = result.Data,
                Message = result.Message
            });
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto data)
        {
            var result = await _authService.RegisterAsync(data);
            return StatusCode((int)result.StatusCode, new ApiResponse<object>
            {
                Data = result.Data,
                Message = result.Message
            });

        }

    }
}