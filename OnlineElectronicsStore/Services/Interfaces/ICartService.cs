using OnlineElectronicsStore.DTOs;
using OnlineElectronicsStore.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OnlineElectronicsStore.Services.Interfaces
{
    public interface ICartService
    {
        // Synchronous methods
        IEnumerable<CartItem> GetCartItems(int userId);
        CartItem? GetCartItemById(int id);
        void AddToCart(CartItem item);
        void UpdateCartItem(CartItem item);
        void RemoveFromCart(int id);
        void ClearCart(int userId);

        // Asynchronous methods for controller use
        Task<bool> AddToCartAsync(CartItemDto item);
        Task<bool> RemoveFromCartAsync(CartItemDto item);
        Task<CartDto> GetCartAsync();
    }
}
