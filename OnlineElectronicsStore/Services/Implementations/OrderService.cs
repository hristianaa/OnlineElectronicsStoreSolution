using OnlineElectronicsStore.Data;
using OnlineElectronicsStore.Models;
using Microsoft.EntityFrameworkCore;
using OnlineElectronicsStore.Data;
using OnlineElectronicsStore.Models;
using OnlineElectronicsStore.Services.Interfaces;

namespace OnlineElectronicsStore.Services.Implementations
{
    public class OrderService : IOrderService
    {
        private readonly AppDbContext _context;

        public OrderService(AppDbContext context)
        {
            _context = context;
        }

        // ✅ Synchronous methods (optional)
        public IEnumerable<Order> GetAll()
        {
            return _context.Orders
                .Include(o => o.User)
                .Include(o => o.CartItems)
                .ToList();
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

        // ✅ Async implementations
        public async Task<IEnumerable<Order>> GetAllAsync()
        {
            return await _context.Orders
                .Include(o => o.User)
                .Include(o => o.OrderItems)
                .ThenInclude(i => i.Product)
                .ToListAsync();
        }

        public async Task<Order?> GetByIdAsync(int id)
        {
            return await _context.Orders
                .Include(o => o.User)
                .Include(o => o.OrderItems)
                .ThenInclude(i => i.Product)
                .FirstOrDefaultAsync(o => o.Id == id);
        }

        public async Task<IEnumerable<Order>> GetByUserIdAsync(int userId)
        {
            return await _context.Orders
                .Include(o => o.OrderItems)
                .ThenInclude(i => i.Product)
                .Where(o => o.UserId == userId)
                .ToListAsync();
        }

        public async Task CreateAsync(Order order)
        {
            _context.Orders.Add(order);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order != null)
            {
                _context.Orders.Remove(order);
                await _context.SaveChangesAsync();
            }
        }
    }
}
