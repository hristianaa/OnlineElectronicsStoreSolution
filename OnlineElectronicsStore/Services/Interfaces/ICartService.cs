using System.Collections.Generic;
using System.Threading.Tasks;
using OnlineElectronicsStore.DTOs;
using OnlineElectronicsStore.Models;

namespace OnlineElectronicsStore.Services.Interfaces
{
    public interface ICartService
    {
        // ─── shopper‐facing/cart API ───────────────────────────────────────────────
        Task<bool> AddToCartAsync(int userId, CartItemDto item);
        Task<bool> RemoveFromCartAsync(int userId, CartItemDto item);
        Task<CartDto> GetCartDtoAsync(int userId);

        // ─── admin/CRUD on CartItem entities ────────────────────────────────────
        Task<IEnumerable<CartItem>> GetCartItemsAsync(int userId);
        Task<CartItem?> GetCartItemByIdAsync(int cartItemId);
        Task<bool> AddCartItemAsync(CartItem model);
        Task<bool> UpdateCartItemAsync(CartItem model);
        Task<bool> RemoveCartItemAsync(int cartItemId);
    }
}

