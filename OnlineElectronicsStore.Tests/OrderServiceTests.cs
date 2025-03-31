using Microsoft.EntityFrameworkCore;
using OnlineElectronicsStore.Data;
using OnlineElectronicsStore.Models;
using OnlineElectronicsStore.Services.Implementations;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace OnlineElectronicsStore.Tests
{
    public class OrderServiceTests
    {
        private AppDbContext GetInMemoryDbContext()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            var context = new AppDbContext(options);
            context.Database.EnsureCreated();

            // Seed user + product + category
            if (!context.Users.Any())
            {
                context.Users.Add(new User { Id = 1, Email = "test@example.com", Password = "1234", FullName = "Test User", Role = "User" });
            }

            if (!context.Categories.Any())
            {
                context.Categories.Add(new Category { Id = 1, Name = "Test Category" });
            }

            if (!context.Products.Any())
            {
                context.Products.Add(new Product { Id = 1, Name = "Test Product", Price = 100, Stock = 5, CategoryId = 1 });
            }

            context.SaveChanges();
            return context;
        }

        [Fact]
        public async Task CreateAsync_ShouldAddOrderToDatabase()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var service = new OrderService(context);

            var order = new Order
            {
                UserId = 1,
                OrderDate = DateTime.UtcNow,
                TotalAmount = 200,
                Status = "Processing", // ✅ Required field
                OrderItems = new[]
                {
                    new OrderItem { ProductId = 1, Quantity = 2, UnitPrice = 100 }
                }
            };

            // Act
            await service.CreateAsync(order);

            // Assert
            Assert.Single(context.Orders);
            Assert.Single(context.OrderItems);
        }

        [Fact]
        public async Task GetByUserIdAsync_ShouldReturnCorrectOrders()
        {
            // Arrange
            var context = GetInMemoryDbContext();

            // Seed orders for two users
            context.Orders.Add(new Order
            {
                Id = 1,
                UserId = 1,
                OrderDate = DateTime.UtcNow,
                TotalAmount = 100,
                Status = "Completed" // ✅ Required field
            });

            context.Orders.Add(new Order
            {
                Id = 2,
                UserId = 99,
                OrderDate = DateTime.UtcNow,
                TotalAmount = 300,
                Status = "Completed" // ✅ Required field
            });

            await context.SaveChangesAsync();

            var service = new OrderService(context);

            // Act
            var result = await service.GetByUserIdAsync(1);

            // Assert
            Assert.Single(result);
            Assert.Equal(1, result.First().UserId);
        }
    }
}
