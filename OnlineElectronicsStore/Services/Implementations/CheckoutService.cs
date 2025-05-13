using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using OnlineElectronicsStore.Data;
using OnlineElectronicsStore.Models;
using OnlineElectronicsStore.Models.ViewModels;
using OnlineElectronicsStore.Services.Interfaces;

namespace OnlineElectronicsStore.Services.Implementations
{
    public class CheckoutService : ICheckoutService
    {
        private readonly AppDbContext _db;
        public CheckoutService(AppDbContext db) => _db = db;

        public async Task<int> PlaceOrderAsync(int userId, CheckoutViewModel vm)
        {
            // fetch cart items
            var cartItems = await _db.CartItems
                .Include(ci => ci.Product)
                .Where(ci => ci.UserId == userId)
                .ToListAsync();

            if (!cartItems.Any())
                throw new InvalidOperationException("Cart is empty");

            // create order
            var order = new Order
            {
                UserId = userId,
                OrderDate = DateTime.UtcNow,
                Status = "Pending",
                TotalAmount = vm.Total
            };
            _db.Orders.Add(order);
            await _db.SaveChangesAsync();

            // create order items
            foreach (var ci in cartItems)
            {
                order.OrderItems.Add(new OrderItem
                {
                    OrderId = order.Id,
                    ProductId = ci.ProductId,
                    Quantity = ci.Quantity,
                    UnitPrice = ci.Product.Price
                });
            }
            await _db.SaveChangesAsync();

            return order.Id;
        }
    }
}
