using OnlineElectronicsStore.DTOs;

namespace OnlineElectronicsStore.Services.Interfaces
{
    public interface ICartService
    {
        Task<CartDto> GetCartAsync(int userId);
        Task<bool> AddToCartAsync(CartItemDto item, int userId);
        Task<bool> RemoveFromCartAsync(int productId, int userId);
        Task ClearCartAsync(int userId);
    }
}
