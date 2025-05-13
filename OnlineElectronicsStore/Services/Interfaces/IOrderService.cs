//using System.Collections.Generic;
using System.Threading.Tasks;
using OnlineElectronicsStore.DTOs;

namespace OnlineElectronicsStore.Services.Interfaces
{
    public interface IOrderService
    {
        /// <summary>
        /// Returns all orders for a given user, mapped to OrderDto.
        /// </summary>
        Task<IEnumerable<OrderDto>> GetOrderHistoryAsync(int userId);

        /// <summary>
        /// Returns one order (with its lines) for a given user, or null.
        /// </summary>
        Task<OrderDto?> GetOrderByIdAsync(int userId, int orderId);

        /// <summary>
        /// Creates a new order entity in the database.
        /// </summary>
        Task<int> CreateOrderAsync(int userId, OrderDto dto);

        /// <summary>
        /// Deletes an order by id; returns true if found and deleted.
        /// </summary>
        Task<bool> DeleteOrderAsync(int userId, int orderId);
    }
}
