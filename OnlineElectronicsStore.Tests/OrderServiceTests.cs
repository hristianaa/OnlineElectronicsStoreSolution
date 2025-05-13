using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using OnlineElectronicsStore.Data;
using OnlineElectronicsStore.DTOs;
using OnlineElectronicsStore.Models;
using OnlineElectronicsStore.Services.Implementations;
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

            // Seed user + category + product
            if (!ctx.Users.Any())
                ctx.Users.Add(new User { Id = 1, Email = "test@example.com", Password = "1234", FullName = "Test User", Role = "User" });

            if (!ctx.Categories.Any())
                ctx.Categories.Add(new Category { Id = 1, Name = "Test Category" });

            if (!ctx.Products.Any())
                ctx.Products.Add(new Product { Id = 1, Name = "Test Product", Price = 100m, Stock = 5, CategoryId = 1 });

            ctx.SaveChanges();
            return ctx;
        }

        [Fact]
        public async Task CreateOrderAsync_ShouldAddOrderAndLines()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var service = new OrderService(context);

            // Build DTO to create
            var dto = new OrderDto
            {
                OrderDate = DateTime.UtcNow,
                Status = "Processing",
                Items = new System.Collections.Generic.List<OrderLineDto>
                {
                    new OrderLineDto { ProductName = "Test Product", Quantity = 2, UnitPrice = 100m }
                }
            };

            // Act
            var newOrderId = await service.CreateOrderAsync(userId: 1, dto);

            // Assert: one order and its items were saved
            var savedOrder = context.Orders
                .Include(o => o.OrderItems)
                .First(o => o.Id == newOrderId);

            Assert.NotNull(savedOrder);
            Assert.Equal(1, savedOrder.UserId);
            Assert.Equal("Processing", savedOrder.Status);

            Assert.Single(savedOrder.OrderItems);
            var savedItem = savedOrder.OrderItems.First();
            Assert.Equal(2, savedItem.Quantity);
            Assert.Equal(100m, savedItem.UnitPrice);
        }

        [Fact]
        public async Task GetOrderHistoryAsync_ShouldReturnOnlyThatUser()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var now = DateTime.UtcNow;

            // Seed two orders: one for user 1, one for user 2
            context.Orders.AddRange(
                new Order
                {
                    UserId = 1,
                    OrderDate = now,
                    Status = "Completed",
                    OrderItems = new[]
                    {
                        new OrderItem { ProductId = 1, Quantity = 1, UnitPrice = 100m }
                    }
                },
                new Order
                {
                    UserId = 2,
                    OrderDate = now,
                    Status = "Completed",
                    OrderItems = new[]
                    {
                        new OrderItem { ProductId = 1, Quantity = 3, UnitPrice = 100m }
                    }
                }
            );
            await context.SaveChangesAsync();

            var service = new OrderService(context);

            // Act
            var historyDtos = (await service.GetOrderHistoryAsync(1)).ToList();

            // Assert
            Assert.Single(historyDtos);
            var first = historyDtos[0];
            Assert.Equal(1, first.Id);               // Order ID auto-assigned
            Assert.Equal("Completed", first.Status);
            Assert.Equal(1 * 100m, first.Items.Sum(i => i.Quantity * i.UnitPrice));
        }
    }
}
