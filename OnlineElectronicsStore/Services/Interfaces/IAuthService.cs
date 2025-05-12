using System.Threading.Tasks;
using OnlineElectronicsStore.Models;
using OnlineElectronicsStore.DTOs;

namespace OnlineElectronicsStore.Services.Interfaces
{
    /// <summary>
    /// Handles user authentication and registration.
    /// </summary>
    public interface IAuthService
    {
        /// <summary>
        /// Validates the given credentials, returning the User if valid, or null otherwise.
        /// </summary>
        Task<User?> ValidateCredentialsAsync(string email, string password);

        /// <summary>
        /// Registers a new user with the given info, returning the created User.
        /// </summary>
        Task<User> RegisterUserAsync(RegisterDto dto);

        /// <summary>
        /// Generates a JWT for the given user.
        /// </summary>
        string GenerateJwtToken(User user);
    }
}
