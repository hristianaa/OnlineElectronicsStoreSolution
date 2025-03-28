using OnlineElectronicsStore.Data;
using OnlineElectronicsStore.Models;
using OnlineElectronicsStore.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace OnlineElectronicsStore.Services.Implementations
{
    public class CartService : ICartService
    {
        private readonly AppDbContext _context;

        public CartService(AppDbContext context)
        {
            _context = context;
        }

        public IEnumerable<CartItem> GetCartItems(int userId)
        {
            return _context.CartItems
                .Include(c => c.Product)
                .Where(c => c.UserId == userId)
                .ToList();
        }

        public CartItem? GetCartItemById(int id)
        {
            return _context.CartItems
                .Include(c => c.Product)
                .FirstOrDefault(c => c.Id == id);
        }

        public void AddToCart(CartItem item)
        {
            _context.CartItems.Add(item);
            _context.SaveChanges();
        }

        public void UpdateCartItem(CartItem item)
        {
            _context.CartItems.Update(item);
            _context.SaveChanges();
        }

        public void RemoveFromCart(int id)
        {
            var item = _context.CartItems.Find(id);
            if (item != null)
            {
                _context.CartItems.Remove(item);
                _context.SaveChanges();
            }
        }

        public void ClearCart(int userId)
        {
            var items = _context.CartItems.Where(c => c.UserId == userId);
            _context.CartItems.RemoveRange(items);
            _context.SaveChanges();
        }
    }
}
