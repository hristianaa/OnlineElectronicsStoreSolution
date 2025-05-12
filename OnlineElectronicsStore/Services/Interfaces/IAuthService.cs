using OnlineElectronicsStore.Models;
using OnlineElectronicsStore.DTOs;
using System.Threading.Tasks;

namespace OnlineElectronicsStore.Services.Interfaces
{
    public interface IAuthService
    {
        Task<User?> ValidateCredentialsAsync(string email, string password);
        string GenerateJwtToken(User user);
        Task<User> RegisterUserAsync(RegisterDto dto);
    }
}
