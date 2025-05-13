using System.Collections.Generic;
using System.Threading.Tasks;
using OnlineElectronicsStore.Models;

namespace OnlineElectronicsStore.Services.Interfaces
{
    public interface IWishlistService
    {
        Task<IEnumerable<Wishlist>> GetByUserIdAsync(int userId);
        Task<bool> AddAsync(int userId, int productId);
        Task<bool> RemoveAsync(int userId, int productId);
    }
}
