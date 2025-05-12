using System.Threading.Tasks;
using OnlineElectronicsStore.DTOs;
using OnlineElectronicsStore.Models;

namespace OnlineElectronicsStore.Services.Interfaces
{
    public interface IAuthService
    {
        /// <summary>
        /// Verifies email & password and returns the matching User, or null.
        /// </summary>
        Task<User?> ValidateCredentialsAsync(string email, string password);

        /// <summary>
        /// Creates a new User from the RegisterDto and returns it (with its new Id).
        /// </summary>
        Task<User> RegisterUserAsync(RegisterDto dto);

        /// <summary>
        /// Generates a JWT for the given user.
        /// </summary>
        string GenerateJwtToken(User user);
    }
}
