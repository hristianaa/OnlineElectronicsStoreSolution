// Services/Interfaces/ICartService.cs
using OnlineElectronicsStore.DTOs;
using OnlineElectronicsStore.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OnlineElectronicsStore.Services.Interfaces
{
    public interface ICartService
    {
        // MVC pages: work with domain CartItem
        Task<IEnumerable<CartItem>> GetCartItemsAsync(int userId);
        Task<CartItem?> GetCartItemByIdAsync(int id);
        Task AddCartItemAsync(CartItem item);
        Task UpdateCartItemAsync(CartItem item);
        Task RemoveCartItemAsync(int id);
        Task ClearCartAsync(int userId);

        // API & AJAX: work with DTOs and aggregate CartDto
        Task<CartDto> GetCartDtoAsync(int userId);
        Task<bool> AddToCartAsync(int userId, CartItemDto item);
        Task<bool> RemoveFromCartAsync(int userId, CartItemDto item);
    }
}

