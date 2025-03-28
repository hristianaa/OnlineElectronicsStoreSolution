using OnlineElectronicsStore.Models;

namespace OnlineElectronicsStore.Services.Interfaces
{
    public interface IOrderService
    {
        IEnumerable<Order> GetAll();
        Order? GetById(int id);
        IEnumerable<Order> GetByUserId(int userId);
        void Create(Order order);
        void Update(Order order);
        void Delete(int id);
    }
}
