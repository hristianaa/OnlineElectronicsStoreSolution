using OnlineElectronicsStore.Models;

namespace OnlineElectronicsStore.Services.Interfaces
{
    public interface ICartService
    {
        IEnumerable<CartItem> GetCartItems(int userId);
        CartItem? GetCartItemById(int id);
        void AddToCart(CartItem item);
        void UpdateCartItem(CartItem item);
        void RemoveFromCart(int id);
        void ClearCart(int userId);
    }
}
