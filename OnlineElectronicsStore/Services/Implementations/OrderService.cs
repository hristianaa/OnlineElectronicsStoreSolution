using OnlineElectronicsStore.Data;
using OnlineElectronicsStore.Models;
using OnlineElectronicsStore.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace OnlineElectronicsStore.Services.Implementations
{
    public class OrderService : IOrderService
    {
        private readonly AppDbContext _context;

        public OrderService(AppDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Order> GetAll()
        {
            return _context.Orders.Include(o => o.User).Include(o => o.CartItems).ToList();
        }

        public Order? GetById(int id)
        {
            return _context.Orders
                .Include(o => o.User)
                .Include(o => o.CartItems)
                .FirstOrDefault(o => o.Id == id);
        }

        public IEnumerable<Order> GetByUserId(int userId)
        {
            return _context.Orders
                .Include(o => o.CartItems)
                .Where(o => o.UserId == userId)
                .ToList();
        }

        public void Create(Order order)
        {
            _context.Orders.Add(order);
            _context.SaveChanges();
        }

        public void Update(Order order)
        {
            _context.Orders.Update(order);
            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            var order = _context.Orders.Find(id);
            if (order != null)
            {
                _context.Orders.Remove(order);
                _context.SaveChanges();
            }
        }
    }
}
