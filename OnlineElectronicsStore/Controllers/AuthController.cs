using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using OnlineElectronicsStore.Data;
using OnlineElectronicsStore.Models;
using OnlineElectronicsStore.DTOs;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace OnlineElectronicsStore.Controllers
{
\[Route("api/\[controller]")]
\[ApiController]
    public class AuthController : ControllerBase
    {
        private readonly AppDbContext \_context;
private readonly IConfiguration \_configuration;

```
    public AuthController(AppDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        // 🔐 LOGIN
        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginDto loginDto)
        {
            var user = _context.Users.FirstOrDefault(u =>
                u.Email == loginDto.Email &&
                u.Password == loginDto.Password);

            if (user == null)
                return Unauthorized("Invalid email or password.");

            var token = GenerateJwtToken(user);
            return Ok(new { token });
        }

        // 🧑 REGISTER
        [HttpPost("register")]
        public IActionResult Register([FromBody] RegisterDto registerDto)
        {
            if (_context.Users.Any(u => u.Email == registerDto.Email))
                return BadRequest("User already exists with this email.");

            var newUser = new User
            {
                FullName = registerDto.FullName,
                Email = registerDto.Email,
                Password = registerDto.Password,
                Role = string.IsNullOrEmpty(registerDto.Role) ? "User" : registerDto.Role
            };

            _context.Users.Add(newUser);
            _context.SaveChanges();

            return Ok("User registered successfully.");
        }

        // 🎟 JWT Token Generator
        private string GenerateJwtToken(User user)
        {
            var jwtSettings = _configuration.GetSection("Jwt");

            var claims = new[]
            {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Role, user.Role)
        };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: jwtSettings["Issuer"],
                audience: jwtSettings["Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(Convert.ToDouble(jwtSettings["ExpiresInMinutes"])),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
```

}
