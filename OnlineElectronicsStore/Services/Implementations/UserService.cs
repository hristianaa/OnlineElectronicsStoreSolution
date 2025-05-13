using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using OnlineElectronicsStore.Data;
using OnlineElectronicsStore.DTOs;
using OnlineElectronicsStore.Models;
using OnlineElectronicsStore.Services.Interfaces;

namespace OnlineElectronicsStore.Services.Implementations
{
    public class UserService : IUserService
    {
        private readonly AppDbContext _context;
        private readonly IPasswordHasher<User> _passwordHasher;

        public UserService(
            AppDbContext context,
            IPasswordHasher<User> passwordHasher)
        {
            _context = context;
            _passwordHasher = passwordHasher;
        }

        public async Task<IEnumerable<User>> GetAllAsync()
        {
            return await _context.Users.ToListAsync();
        }

        public async Task<User?> GetByIdAsync(int id)
        {
            return await _context.Users.FindAsync(id);
        }

        public async Task<User?> GetByEmailAsync(string email)
        {
            return await _context.Users
                       .FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<User> CreateAsync(User user)
        {
            // Hash the password before saving
            user.Password = _passwordHasher.HashPassword(user, user.Password);
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task<bool> UpdateAsync(User user)
        {
            var existing = await _context.Users.FindAsync(user.Id);
            if (existing == null) return false;

            existing.FullName = user.FullName;
            existing.Email = user.Email;
            existing.Role = user.Role;
            // If Password property is set to a new hashed value before calling
            existing.Password = user.Password;

            _context.Users.Update(existing);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null) return false;
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return true;
        }

        /// <summary>
        /// Updates profile: name, email, and optionally password (hashed).
        /// </summary>
        public async Task<bool> UpdateProfileAsync(UpdateUserDto dto)
        {
            var existing = await _context.Users.FindAsync(dto.Id);
            if (existing == null) return false;

            existing.FullName = dto.FullName;
            existing.Email = dto.Email;

            if (!string.IsNullOrEmpty(dto.NewPassword))
            {
                existing.Password = _passwordHasher.HashPassword(existing, dto.NewPassword);
            }

            _context.Users.Update(existing);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}