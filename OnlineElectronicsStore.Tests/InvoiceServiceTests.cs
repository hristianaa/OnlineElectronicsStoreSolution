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
    public class InvoiceServiceTests
    {
        private AppDbContext GetInMemoryDbContext()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .EnableSensitiveDataLogging()
                .Options;

            var context = new AppDbContext(options);
            context.Database.EnsureCreated();

            if (!context.Users.Any())
            {
                context.Users.Add(new User { Id = 1, Email = "test@example.com", Password = "123", FullName = "Test", Role = "User" });
            }

            if (!context.Categories.Any())
            {
                context.Categories.Add(new Category { Id = 1, Name = "TestCat" });
            }

            if (!context.Products.Any())
            {
                context.Products.Add(new Product { Id = 1, Name = "Product A", Price = 100, Stock = 5, CategoryId = 1 });
            }

            context.SaveChanges();
            return context;
        }

        [Fact]
        public async Task CreateAsync_ShouldAddInvoiceToDatabase()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var service = new InvoiceService(context);

            var order = new Order
            {
                UserId = 1,
                OrderDate = DateTime.UtcNow,
                Status = "Completed",
                TotalAmount = 200,
                OrderItems = new[]
                {
                    new OrderItem { ProductId = 1, Quantity = 2, UnitPrice = 100 }
                }
            };

            context.Orders.Add(order);
            await context.SaveChangesAsync();

            var invoice = new Invoice
            {
                OrderId = order.Id,
                TotalAmount = 200
            };

            // Act
            var result = await service.CreateAsync(invoice);

            // Assert
            Assert.NotNull(result);
            Assert.Single(context.Invoices);
            Assert.Equal(order.Id, result.OrderId);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnInvoiceWithOrderItems()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var order = new Order
            {
                UserId = 1,
                OrderDate = DateTime.UtcNow,
                Status = "Completed",
                TotalAmount = 200,
                OrderItems = new[]
                {
                    new OrderItem { ProductId = 1, Quantity = 1, UnitPrice = 100 }
                }
            };
            context.Orders.Add(order);
            await context.SaveChangesAsync();

            var invoice = new Invoice
            {
                OrderId = order.Id,
                TotalAmount = 200
            };
            context.Invoices.Add(invoice);
            await context.SaveChangesAsync();

            var service = new InvoiceService(context);

            // Act
            var fetched = await service.GetByIdAsync(invoice.Id);

            // Assert
            Assert.NotNull(fetched);
            Assert.Single(fetched.Order.OrderItems);
        }
    }
}
