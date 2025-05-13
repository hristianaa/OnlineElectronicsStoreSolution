using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using OnlineElectronicsStore.Data;
using OnlineElectronicsStore.DTOs;
using OnlineElectronicsStore.Models;
using OnlineElectronicsStore.Services.Implementations;
using Xunit;

namespace OnlineElectronicsStore.Tests.Services
{
    public class CartServiceTests
    {
        private AppDbContext GetInMemoryDbContext()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: $"CartDb_{System.Guid.NewGuid()}")
                .Options;

            // seed with one product
            var ctx = new AppDbContext(options);
            ctx.Products.Add(new Product { Id = 1, Name = "Widget", Price = 9.99m });
            ctx.SaveChanges();
            return ctx;
        }

        [Fact]
        public async Task AddToCartAsync_NewItem_InsertsCartItem()
        {
            // Arrange
            var ctx = GetInMemoryDbContext();
            var service = new CartService(ctx);
            var dto = new CartItemDto { ProductId = 1, Quantity = 2 };

            // Act
            var added = await service.AddToCartAsync(userId: 42, item: dto);

            // Assert
            Assert.True(added);
            var dbItem = ctx.CartItems.Single();
            Assert.Equal(42, dbItem.UserId);
            Assert.Equal(1, dbItem.ProductId);
            Assert.Equal(2, dbItem.Quantity);
        }

        [Fact]
        public async Task AddToCartAsync_ExistingItem_IncrementsQuantity()
        {
            // Arrange
            var ctx = GetInMemoryDbContext();
            ctx.CartItems.Add(new CartItem { UserId = 42, ProductId = 1, Quantity = 1 });
            ctx.SaveChanges();

            var service = new CartService(ctx);
            var dto = new CartItemDto { ProductId = 1, Quantity = 3 };

            // Act
            var added = await service.AddToCartAsync(userId: 42, item: dto);

            // Assert
            Assert.True(added);
            var dbItem = ctx.CartItems.Single();
            Assert.Equal(4, dbItem.Quantity); // 1 + 3
        }

        [Fact]
        public async Task RemoveFromCartAsync_ExistingItem_RemovesIt()
        {
            // Arrange
            var ctx = GetInMemoryDbContext();
            ctx.CartItems.Add(new CartItem { UserId = 42, ProductId = 1, Quantity = 5 });
            ctx.SaveChanges();

            var service = new CartService(ctx);
            var dto = new CartItemDto { ProductId = 1, Quantity = 0 };

            // Act
            var removed = await service.RemoveFromCartAsync(userId: 42, item: dto);

            // Assert
            Assert.True(removed);
            Assert.Empty(ctx.CartItems);
        }

        [Fact]
        public async Task GetCartDtoAsync_ReturnsDtoWithCorrectTotals()
        {
            // Arrange
            var ctx = GetInMemoryDbContext();
            ctx.CartItems.AddRange(
                new CartItem { UserId = 42, ProductId = 1, Quantity = 2 },
                new CartItem { UserId = 42, ProductId = 1, Quantity = 3 }
            );
            ctx.SaveChanges();

            var service = new CartService(ctx);

            // Act
            var dto = await service.GetCartDtoAsync(userId: 42);

            // Assert
            // Should coalesce into one CartLineDto with Quantity = 5
            Assert.Single(dto.Items);
            var line = dto.Items.Single();
            Assert.Equal(1, line.ProductId);
            Assert.Equal(5, line.Quantity);
            Assert.Equal(9.99m, line.UnitPrice);
            Assert.Equal(5 * 9.99m, dto.TotalPrice);
        }
    }
}
