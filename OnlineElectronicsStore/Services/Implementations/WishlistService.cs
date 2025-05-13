using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using OnlineElectronicsStore.Data;
using OnlineElectronicsStore.Models;
using OnlineElectronicsStore.Services.Interfaces;

namespace OnlineElectronicsStore.Services.Implementations
{
    public class WishlistService : IWishlistService
    {
        private readonly AppDbContext _db;
        public WishlistService(AppDbContext db) => _db = db;

        public async Task<IEnumerable<Wishlist>> GetByUserIdAsync(int userId)
        {
            return await _db.Wishlists
                            .Include(w => w.Product)
                            .Where(w => w.UserId == userId)
                            .ToListAsync();
        }

        public async Task<bool> AddAsync(int userId, int productId)
        {
            if (await _db.Wishlists.AnyAsync(w => w.UserId == userId && w.ProductId == productId))
                return false;

            _db.Wishlists.Add(new Wishlist { UserId = userId, ProductId = productId });
            await _db.SaveChangesAsync();
            return true;
        }

        public async Task<bool> RemoveAsync(int userId, int productId)
        {
            var item = await _db.Wishlists
                                .FirstOrDefaultAsync(w => w.UserId == userId && w.ProductId == productId);
            if (item == null) return false;

            _db.Wishlists.Remove(item);
            await _db.SaveChangesAsync();
            return true;
        }
    }
}
