using OnlineElectronicsStore.Data;
using OnlineElectronicsStore.Data;
using OnlineElectronicsStore.DTOs;
using OnlineElectronicsStore.Services.Interfaces;
using OnlineElectronicsStore.Models;  // if you have CartItem model
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Text.Json;

namespace OnlineElectronicsStore.Services.Implementations
{
    public class CartService : ICartService
    {
        private readonly AppDbContext _context;

        public CartService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<CartDto> GetCartAsync(int userId)
        {
            var cartItems = await _context.CartItems
                .Include(ci => ci.Product)
                .Where(ci => ci.UserId == userId)
                .ToListAsync();

            var dto = new CartDto
            {
                Items = cartItems.Select(ci => new CartItemDto
                {
                    ProductId = ci.ProductId,
                    Quantity = ci.Quantity,
                }).ToList(),
                TotalPrice = cartItems.Sum(ci => ci.Quantity * ci.Product.Price)
            };

            return dto;
        }

        public async Task<bool> AddToCartAsync(CartItemDto item, int userId)
        {
            var product = await _context.Products.FindAsync(item.ProductId);
            if (product == null)
                return false;

            var cartItem = await _context.CartItems
                .FirstOrDefaultAsync(ci => ci.ProductId == item.ProductId && ci.UserId == userId);

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

        public async Task<bool> RemoveFromCartAsync(int productId, int userId)
        {
            var cartItem = await _context.CartItems
                .FirstOrDefaultAsync(ci => ci.ProductId == productId && ci.UserId == userId);

            if (cartItem == null)
                return false;

            _context.CartItems.Remove(cartItem);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task ClearCartAsync(int userId)
        {
            var cartItems = await _context.CartItems
                .Where(ci => ci.UserId == userId)
                .ToListAsync();

            _context.CartItems.RemoveRange(cartItems);
            await _context.SaveChangesAsync();
        }
    }
}
