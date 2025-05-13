using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using OnlineElectronicsStore.Data;
using OnlineElectronicsStore.DTOs;
using OnlineElectronicsStore.Models;
using OnlineElectronicsStore.Services.Interfaces;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace OnlineElectronicsStore.Services.Implementations
{
    public class AuthService : IAuthService
    {
        private readonly AppDbContext _ctx;
        private readonly IConfiguration _config;

        public AuthService(AppDbContext ctx, IConfiguration config)
        {
            _ctx = ctx;
            _config = config;
        }

        public async Task<User?> ValidateCredentialsAsync(string email, string password)
        {
            // in prod you’d hash & compare
            return await Task.FromResult(
                _ctx.Users.FirstOrDefault(u => u.Email == email && u.Password == password));
        }

        public async Task<User> RegisterUserAsync(RegisterDto dto)
        {
            var user = new User
            {
                FullName = dto.FullName,
                Email = dto.Email,
                Password = dto.Password,  // hash in prod!
                Role = dto.Role
            };
            _ctx.Users.Add(user);
            await _ctx.SaveChangesAsync();
            return user;
        }

        public string GenerateJwtToken(User user)
        {
            var jwt = _config.GetSection("Jwt");
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt["Key"]!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email,          user.Email),
                new Claim(ClaimTypes.Role,           user.Role)
            };

            var token = new JwtSecurityToken(
                issuer: jwt["Issuer"],
                audience: jwt["Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(double.Parse(jwt["ExpiresInMinutes"]!)),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task SignInWithCookieAsync(HttpContext httpContext, User user)
        {
            // Build the claims
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name,           user.FullName),
                new Claim(ClaimTypes.Email,          user.Email),
                new Claim(ClaimTypes.Role,           user.Role),
            };
            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);

            // Actually sign in
            await httpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                principal,
                new AuthenticationProperties
                {
                    IsPersistent = true,
                    ExpiresUtc = DateTimeOffset.UtcNow.AddHours(1)
                });
        }
    }
}
