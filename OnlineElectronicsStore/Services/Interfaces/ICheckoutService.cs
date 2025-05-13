using System.Threading.Tasks;
using OnlineElectronicsStore.Models.ViewModels;

namespace OnlineElectronicsStore.Services.Interfaces
{
    public interface ICheckoutService
    {
        /// <summary>
        /// Creates an Order record from the user's cart and checkout VM, 
        /// returns the new Order.Id
        /// </summary>
        Task<int> PlaceOrderAsync(int userId, CheckoutViewModel checkout);
    }
}
