// Services/Interfaces/IOrderService.cs
using OnlineElectronicsStore.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OnlineElectronicsStore.Services.Interfaces
{
    public interface IOrderService
    {
        /// <summary>
        /// Returns all orders, including user and line-item details.
        /// </summary>
        Task<IEnumerable<Order>> GetAllAsync();

        /// <summary>
        /// Returns a single order by ID, or null if not found.
        /// </summary>
        Task<Order?> GetByIdAsync(int id);

        /// <summary>
        /// Returns all orders placed by a specific user.
        /// </summary>
        Task<IEnumerable<Order>> GetByUserIdAsync(int userId);

        /// <summary>
        /// Creates a new order.
        /// </summary>
        Task CreateAsync(Order order);

        /// <summary>
        /// Deletes the order with the given ID. 
        /// Returns true if the order existed and was deleted.
        /// </summary>
        Task<bool> DeleteAsync(int id);

    }
}

