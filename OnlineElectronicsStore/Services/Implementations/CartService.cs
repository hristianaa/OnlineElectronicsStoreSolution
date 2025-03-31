using OnlineElectronicsStore.Data;
using OnlineElectronicsStore.Models;
using OnlineElectronicsStore.Services.Interfaces;
using OnlineElectronicsStore.DTOs;
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

        // Sync methods
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

        // Async methods for API use

        public async Task<bool> AddToCartAsync(CartItemDto item)
        {
            var product = await _context.Products.FindAsync(item.ProductId);
            if (product == null) return false;

            var cartItem = await _context.CartItems
                .FirstOrDefaultAsync(c => c.ProductId == item.ProductId && c.UserId == 1); // TEMP userId

            if (cartItem != null)
            {
                cartItem.Quantity += item.Quantity;
            }
            else
            {
                cartItem = new CartItem
                {
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                    UserId = 1 // TEMP userId
                };
                _context.CartItems.Add(cartItem);
            }

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> RemoveFromCartAsync(CartItemDto item)
        {
            var cartItem = await _context.CartItems
                .FirstOrDefaultAsync(c => c.ProductId == item.ProductId && c.UserId == 1); // TEMP userId

            if (cartItem != null)
            {
                _context.CartItems.Remove(cartItem);
                await _context.SaveChangesAsync();
                return true;
            }

            return false;
        }

        public async Task<CartDto> GetCartAsync()
        {
            var items = await _context.CartItems
                .Include(c => c.Product)
                .Where(c => c.UserId == 1) // TEMP userId
                .ToListAsync();

            var dto = new CartDto
            {
                Items = items.Select(c => new CartItemDto
                {
                    ProductId = c.ProductId,
                    Quantity = c.Quantity
                }).ToList(),
                TotalPrice = items.Sum(c => c.Quantity * c.Product.Price)
            };

            return dto;
        }
    }
}
