// Services/Implementations/CartService.cs
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
        private readonly AppDbContext _context;
        public CartService(AppDbContext context) => _context = context;

        // MVC: domain operations

        public async Task<IEnumerable<CartItem>> GetCartItemsAsync(int userId)
        {
            return await _context.CartItems
                                 .Include(c => c.Product)
                                 .Where(c => c.UserId == userId)
                                 .ToListAsync();
        }

        public async Task<CartItem?> GetCartItemByIdAsync(int id)
        {
            return await _context.CartItems
                                 .Include(c => c.Product)
                                 .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task AddCartItemAsync(CartItem item)
        {
            _context.CartItems.Add(item);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateCartItemAsync(CartItem item)
        {
            _context.CartItems.Update(item);
            await _context.SaveChangesAsync();
        }

        public async Task RemoveCartItemAsync(int id)
        {
            var item = await _context.CartItems.FindAsync(id);
            if (item != null)
            {
                _context.CartItems.Remove(item);
                await _context.SaveChangesAsync();
            }
        }

        public async Task ClearCartAsync(int userId)
        {
            var items = _context.CartItems.Where(c => c.UserId == userId);
            _context.CartItems.RemoveRange(items);
            await _context.SaveChangesAsync();
        }

        // API/AJAX: DTO operations

        public async Task<CartDto> GetCartDtoAsync(int userId)
        {
            var items = await _context.CartItems
                                      .Include(c => c.Product)
                                      .Where(c => c.UserId == userId)
                                      .ToListAsync();

            return new CartDto
            {
                Items = items.Select(c => new CartItemDto
                {
                    ProductId = c.ProductId,
                    Quantity = c.Quantity
                }).ToList(),
                TotalPrice = items.Sum(c => c.Quantity * c.Product.Price)
            };
        }

        public async Task<bool> AddToCartAsync(int userId, CartItemDto item)
        {
            var product = await _context.Products.FindAsync(item.ProductId);
            if (product == null) return false;

            var cartItem = await _context.CartItems
                .FirstOrDefaultAsync(c => c.ProductId == item.ProductId && c.UserId == userId);

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
                    UserId = userId
                };
                _context.CartItems.Add(cartItem);
            }

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> RemoveFromCartAsync(int userId, CartItemDto item)
        {
            var cartItem = await _context.CartItems
                .FirstOrDefaultAsync(c => c.ProductId == item.ProductId && c.UserId == userId);

            if (cartItem != null)
            {
                _context.CartItems.Remove(cartItem);
                await _context.SaveChangesAsync();
                return true;
            }

            return false;
        }
    }
}
