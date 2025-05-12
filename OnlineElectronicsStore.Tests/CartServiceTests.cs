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

            // Seed a user
            if (!context.Users.Any())
            {
                context.Users.Add(new User
                {
                    Id = 1,
                    Email = "test@example.com",
                    Password = "1234",
                    FullName = "Test",
                    Role = "User"
                });
            }

            // Seed a category
            if (!context.Categories.Any())
            {
                context.Categories.Add(new Category { Id = 1, Name = "TestCat" });
            }

            // Seed a product
            if (!context.Products.Any())
            {
                context.Products.Add(new Product
                {
                    Id = 1,
                    Name = "Test Product",
                    ShortDescription = "Test",
                    LongDescription = "Test product long description",
                    MainImageUrl = "http://example.com/img.jpg",
                    Price = 50m,
                    Stock = 20,
                    CategoryId = 1
                });
            }

            context.SaveChanges();
            return context;
        }

        [Fact]
        public async Task AddToCartAsync_ShouldAddItemToCart()
        {
            var ctx = GetInMemoryDbContext();
            var service = new CartService(ctx);

            var dto = new CartItemDto
            {
                ProductId = 1,
                Quantity = 2
            };

            var result = await service.AddToCartAsync(
                userId: 1,
                item: dto
            );

            Assert.True(result);
            Assert.Equal(1, ctx.CartItems.Count());
        }

        [Fact]
        public async Task RemoveFromCartAsync_ShouldRemoveItem()
        {
            var ctx = GetInMemoryDbContext();
            ctx.CartItems.Add(new CartItem
            {
                ProductId = 1,
                Quantity = 2,
                UserId = 1
            });
            await ctx.SaveChangesAsync();

            var service = new CartService(ctx);
            var dto = new CartItemDto { ProductId = 1, Quantity = 2 };

            var result = await service.RemoveFromCartAsync(
                userId: 1,
                item: dto
            );

            Assert.True(result);
            Assert.Empty(ctx.CartItems);
        }

        [Fact]
        public async Task GetCartDtoAsync_ShouldReturnCart()
        {
            var ctx = GetInMemoryDbContext();
            ctx.CartItems.Add(new CartItem
            {
                ProductId = 1,
                Quantity = 1,
                UserId = 1
            });
            await ctx.SaveChangesAsync();

            var service = new CartService(ctx);
            var cart = await service.GetCartDtoAsync(userId: 1);

            Assert.NotNull(cart);
            Assert.Single(cart.Items);
        }
    }
}
