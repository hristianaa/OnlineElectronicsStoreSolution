using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using OnlineElectronicsStore.Data;
using OnlineElectronicsStore.DTOs;
using OnlineElectronicsStore.Models;
using OnlineElectronicsStore.Services.Interfaces;

namespace OnlineElectronicsStore.Services.Implementations
{
    public class OrderService : IOrderService
    {
        private readonly AppDbContext _context;
        public OrderService(AppDbContext context) => _context = context;

        public async Task<IEnumerable<OrderDto>> GetOrderHistoryAsync(int userId)
        {
            var orders = await _context.Orders
                .Where(o => o.UserId == userId)
                .Include(o => o.OrderItems)
                  .ThenInclude(oi => oi.Product)
                .OrderByDescending(o => o.OrderDate)
                .ToListAsync();

            return orders.Select(o => new OrderDto
            {
                Id = o.Id,
                OrderDate = o.OrderDate,
                Status = o.Status,
                Items = o.OrderItems.Select(oi => new OrderLineDto
                {
                    ProductName = oi.Product.Name,
                    Quantity = oi.Quantity,
                    UnitPrice = oi.UnitPrice
                }).ToList()
            });
        }

        public async Task<OrderDto?> GetOrderByIdAsync(int userId, int orderId)
        {
            var o = await _context.Orders
                .Where(o2 => o2.UserId == userId && o2.Id == orderId)
                .Include(o2 => o2.OrderItems)
                  .ThenInclude(oi => oi.Product)
                .FirstOrDefaultAsync();

            if (o == null) return null;

            return new OrderDto
            {
                Id = o.Id,
                OrderDate = o.OrderDate,
                Status = o.Status,
                Items = o.OrderItems.Select(oi => new OrderLineDto
                {
                    ProductName = oi.Product.Name,
                    Quantity = oi.Quantity,
                    UnitPrice = oi.UnitPrice
                }).ToList()
            };
        }

        public async Task<int> CreateOrderAsync(int userId, OrderDto dto)
        {
            // map DTO → EF Order
            var order = new Order
            {
                UserId = userId,
                OrderDate = dto.OrderDate != default
                              ? dto.OrderDate
                              : System.DateTime.UtcNow,
                Status = dto.Status,
                OrderItems = dto.Items.Select(i => new OrderItem
                {
                    ProductId = _context.Products
                                          .Where(p => p.Name == i.ProductName)
                                          .Select(p => p.Id)
                                          .FirstOrDefault(),
                    Quantity = i.Quantity,
                    UnitPrice = i.UnitPrice
                }).ToList()
            };

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();
            return order.Id;
        }

        public async Task<bool> DeleteOrderAsync(int userId, int orderId)
        {
            var order = await _context.Orders
                .FirstOrDefaultAsync(o => o.Id == orderId && o.UserId == userId);
            if (order == null) return false;

            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
