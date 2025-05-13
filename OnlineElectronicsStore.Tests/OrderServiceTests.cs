using Microsoft.EntityFrameworkCore;
using OnlineElectronicsStore.Data;
using OnlineElectronicsStore.DTOs;
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
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var ctx = new AppDbContext(options);
            ctx.Database.EnsureCreated();

            // Seed a user, category, product
            if (!ctx.Users.Any())
                ctx.Users.Add(new Models.User { Id = 1, Email = "test@example.com", Password = "1234", FullName = "Test", Role = "User" });
            if (!ctx.Categories.Any())
                ctx.Categories.Add(new Models.Category { Id = 1, Name = "Test Cat" });
            if (!ctx.Products.Any())
                ctx.Products.Add(new Models.Product { Id = 1, Name = "Prod", Price = 50, Stock = 10, CategoryId = 1 });

            ctx.SaveChanges();
            return ctx;
        }

        [Fact]
        public async Task CreateOrderAsync_ShouldAddOrderAndLines()
        {
            // Arrange
            var ctx = GetInMemoryDbContext();
            var svc = new OrderService(ctx);

            var dto = new OrderDto
            {
                OrderDate = new DateTime(2025, 5, 14, 12, 0, 0),
                Status = "Processing",
                Items = new System.Collections.Generic.List<OrderLineDto> {
                    new OrderLineDto { ProductName = "Prod", Quantity = 2, UnitPrice = 50 }
                }
            };

            // Act
            int newId = await svc.CreateOrderAsync(userId: 1, dto);

            // Assert: one order saved with correct fields
            var saved = ctx.Orders.Include(o => o.OrderItems).FirstOrDefault(o => o.Id == newId);
            Assert.NotNull(saved);
            Assert.Equal(1, saved.UserId);
            Assert.Equal("Processing", saved.Status);
            Assert.Single(saved.OrderItems);
            Assert.Equal(2, saved.OrderItems[0].Quantity);
            Assert.Equal(50m, saved.OrderItems[0].UnitPrice);
        }

        [Fact]
        public async Task GetOrderHistoryAsync_ShouldReturnOnlyThatUser()
        {
            // Arrange
            var ctx = GetInMemoryDbContext();

            // Seed two orders for different users
            ctx.Orders.AddRange(
                new Models.Order
                {
                    UserId = 1,
                    OrderDate = DateTime.UtcNow,
                    Status = "Done",
                    OrderItems = new[] {
                        new Models.OrderItem { ProductId=1, Quantity=1, UnitPrice=50 }
                    }
                },
                new Models.Order
                {
                    UserId = 2,
                    OrderDate = DateTime.UtcNow,
                    Status = "Done",
                    OrderItems = new[] {
                        new Models.OrderItem { ProductId=1, Quantity=3, UnitPrice=50 }
                    }
                }
            );
            await ctx.SaveChangesAsync();

            var svc = new OrderService(ctx);

            // Act
            var history = (await svc.GetOrderHistoryAsync(1)).ToList();

            // Assert
            Assert.Single(history);
            var first = history[0];
            Assert.Equal(1, first.Id);
            Assert.Equal("Done", first.Status);
            Assert.Equal(1 * 50m, first.Items.Sum(i => i.Quantity * i.UnitPrice));
        }
    }
}
