using OnlineElectronicsStore.Models;

namespace OnlineElectronicsStore.Services.Interfaces
{
    public interface IOrderService
    {
        // Optional: synchronous methods (used elsewhere?)
        IEnumerable<Order> GetAll();
        Order? GetById(int id);
        IEnumerable<Order> GetByUserId(int userId);
        void Create(Order order);
        void Update(Order order);
        void Delete(int id);

        // ✅ Required async methods (for updated controller)
        Task<IEnumerable<Order>> GetAllAsync();
        Task<Order?> GetByIdAsync(int id);
        Task<IEnumerable<Order>> GetByUserIdAsync(int userId);
        Task CreateAsync(Order order);
        Task DeleteAsync(int id);
    }
}
