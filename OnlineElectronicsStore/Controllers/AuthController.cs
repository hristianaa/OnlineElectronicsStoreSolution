using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using OnlineElectronicsStore.DTOs;
using OnlineElectronicsStore.Models;
using OnlineElectronicsStore.Services.Interfaces;

namespace OnlineElectronicsStore.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IUserService _userService;

        public AuthController(IAuthService authService, IUserService userService)
        {
            _authService = authService;
            _userService = userService;
        }

        // 🔐 LOGIN
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            var user = await _authService.ValidateCredentialsAsync(loginDto.Email, loginDto.Password);
            if (user == null)
                return Unauthorized(new { Message = "Invalid email or password." });

            var token = _authService.GenerateJwtToken(user);
            return Ok(new { Token = token });
        }

        // 🧑 REGISTER
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
        {
            // Check for existing user with this email
            if (await _userService.GetByEmailAsync(registerDto.Email) != null)
                return BadRequest(new { Message = "User already exists with this email." });

            var newUser = await _authService.RegisterUserAsync(registerDto);
            return Ok(new { Message = "User registered successfully.", UserId = newUser.Id });
        }
    }
}
