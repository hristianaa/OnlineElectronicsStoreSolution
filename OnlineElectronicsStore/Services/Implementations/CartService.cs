using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using OnlineElectronicsStore.Data;
using OnlineElectronicsStore.DTOs;
using OnlineElectronicsStore.Models;
using OnlineElectronicsStore.Services.Interfaces;

namespace OnlineElectronicsStore.Services.Implementations
{
    public class CartService : ICartService
    {
        private readonly AppDbContext _db;
        public CartService(AppDbContext db) => _db = db;

        // ─── shopper‐facing/cart API ───────────────────────────────────────────────
        public async Task<bool> AddToCartAsync(int userId, CartItemDto item)
        {
            var existing = await _db.CartItems
                .FirstOrDefaultAsync(ci => ci.UserId == userId && ci.ProductId == item.ProductId);

            if (existing != null)
            {
                existing.Quantity += item.Quantity;
                _db.CartItems.Update(existing);
            }
            else
            {
                _db.CartItems.Add(new CartItem
                {
                    UserId = userId,
                    ProductId = item.ProductId,
                    Quantity = item.Quantity
                });
            }

            return await _db.SaveChangesAsync() > 0;
        }

        public async Task<bool> RemoveFromCartAsync(int userId, CartItemDto item)
        {
            var existing = await _db.CartItems
                .FirstOrDefaultAsync(ci => ci.UserId == userId && ci.ProductId == item.ProductId);
            if (existing == null) return false;

            _db.CartItems.Remove(existing);
            return await _db.SaveChangesAsync() > 0;
        }

        public async Task<CartDto> GetCartDtoAsync(int userId)
        {
            var cartItems = await _db.CartItems
                .Include(ci => ci.Product)
                .Where(ci => ci.UserId == userId)
                .ToListAsync();

            var dto = new CartDto();
            foreach (var ci in cartItems)
            {
                dto.Items.Add(new CartLineDto
                {
                    ProductId = ci.ProductId,
                    ProductName = ci.Product!.Name,
                    UnitPrice = ci.Product.Price,
                    Quantity = ci.Quantity
                });
            }

            return dto;
        }

        // ─── admin/CRUD on CartItem entities ────────────────────────────────────
        public async Task<IEnumerable<CartItem>> GetCartItemsAsync(int userId)
            => await _db.CartItems
                        .Include(ci => ci.Product)
                        .Where(ci => ci.UserId == userId)
                        .ToListAsync();

        public async Task<CartItem?> GetCartItemByIdAsync(int cartItemId)
            => await _db.CartItems
                        .Include(ci => ci.Product)
                        .FirstOrDefaultAsync(ci => ci.Id == cartItemId);

        public async Task<bool> AddCartItemAsync(CartItem model)
        {
            _db.CartItems.Add(model);
            return await _db.SaveChangesAsync() > 0;
        }

        public async Task<bool> UpdateCartItemAsync(CartItem model)
        {
            _db.CartItems.Update(model);
            return await _db.SaveChangesAsync() > 0;
        }

        public async Task<bool> RemoveCartItemAsync(int cartItemId)
        {
            var ci = await _db.CartItems.FindAsync(cartItemId);
            if (ci == null) return false;
            _db.CartItems.Remove(ci);
            return await _db.SaveChangesAsync() > 0;
        }
    }
}
