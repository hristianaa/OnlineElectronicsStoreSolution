// Services/Interfaces/IUserService.cs
using OnlineElectronicsStore.DTOs;
using OnlineElectronicsStore.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OnlineElectronicsStore.Services.Interfaces
{
    public interface IUserService
    {
        /// <summary>
        /// Returns all users.
        /// </summary>
        Task<IEnumerable<User>> GetAllAsync();

        /// <summary>
        /// Returns a single user by ID, or null if not found.
        /// </summary>
        Task<User?> GetByIdAsync(int id);

        /// <summary>
        /// Returns a single user by email, or null if not found.
        /// </summary>
        Task<User?> GetByEmailAsync(string email);

        /// <summary>
        /// Creates a new user and returns the created entity (with its new ID).
        /// </summary>
        Task<User> CreateAsync(User user);

        /// <summary>
        /// Updates an existing user; returns true if the update succeeded.
        /// </summary>
        Task<bool> UpdateAsync(User user);

        /// <summary>
        /// Deletes the user with the given ID; returns true if deleted.
        /// </summary>
        Task<bool> DeleteAsync(int id);
        
        /// <summary>
        /// Updates a user's profile (name, email, optional password). Returns true on success.
        /// </summary>
        Task<bool> UpdateProfileAsync(UpdateUserDto dto);
    }
}
