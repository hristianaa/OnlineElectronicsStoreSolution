using Microsoft.EntityFrameworkCore;
using OnlineElectronicsStore.Data;
using OnlineElectronicsStore.DTOs;
using OnlineElectronicsStore.Models;
using OnlineElectronicsStore.Services.Implementations;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace OnlineElectronicsStore.Tests
{
    public class CartServiceTests
    {
        private AppDbContext GetInMemoryDbContext()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var context = new AppDbContext(options);
            context.Database.EnsureCreated();

            // Seed dependencies
            if (!context.Users.Any())
            {
                context.Users.Add(new User { Id = 1, Email = "test@example.com", Password = "1234", FullName = "Test", Role = "User" });
            }

            if (!context.Products.Any())
            {
                context.Products.Add(new Product { Id = 1, Name = "Test Product", Price = 50, Stock = 20, CategoryId = 1 });
            }

            if (!context.Categories.Any())
            {
                context.Categories.Add(new Category { Id = 1, Name = "TestCat" });
            }

            context.SaveChanges();
            return context;
        }

        [Fact]
        public async Task AddToCartAsync_ShouldAddItemToCart()
        {
            var context = GetInMemoryDbContext();
            var service = new CartService(context);

            var cartItemDto = new CartItemDto
            {
                ProductId = 1,
                Quantity = 2
            };

            var result = await service.AddToCartAsync(cartItemDto);

            Assert.True(result);
            Assert.Equal(1, context.CartItems.Count());
        }

        [Fact]
        public async Task RemoveFromCartAsync_ShouldRemoveItem()
        {
            var context = GetInMemoryDbContext();

            // Seed cart item
            context.CartItems.Add(new CartItem
            {
                ProductId = 1,
                Quantity = 2,
                UserId = 1
            });
            await context.SaveChangesAsync();

            var service = new CartService(context);

            var dto = new CartItemDto
            {
                ProductId = 1,
                Quantity = 2
            };

            await service.RemoveFromCartAsync(dto);

            Assert.Empty(context.CartItems);
        }

        [Fact]
        public async Task GetCartAsync_ShouldReturnCart()
        {
            var context = GetInMemoryDbContext();
            context.CartItems.Add(new CartItem
            {
                ProductId = 1,
                Quantity = 1,
                UserId = 1
            });
            await context.SaveChangesAsync();

            var service = new CartService(context);
            var cart = await service.GetCartAsync();

            Assert.NotNull(cart);
            Assert.Single(cart.Items);
        }
    }
}
