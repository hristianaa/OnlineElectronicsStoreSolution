// Services/Interfaces/ICheckoutService.cs
using System.Threading.Tasks;

namespace OnlineElectronicsStore.Services.Interfaces
{
    /// <summary>
    /// Handles converting a user’s cart into an order.
    /// </summary>
    public interface ICheckoutService
    {
        /// <summary>
        /// Creates an order for the given user from their cart items,
        /// then clears those items out of the cart.
        /// Returns true on success, false if the cart was empty.
        /// </summary>
        Task<bool> PlaceOrderAsync(int userId);
    }
}
