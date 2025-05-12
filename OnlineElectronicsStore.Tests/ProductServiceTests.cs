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
    public class ProductServiceTests
    {
        // Creates an isolated in-memory DB for each test
        private AppDbContext GetInMemoryDbContext()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var context = new AppDbContext(options);
            context.Database.EnsureCreated();

            // Seed a category if missing
            if (!context.Categories.Any())
            {
                context.Categories.Add(new Category { Id = 1, Name = "Test Category" });
                context.SaveChanges();
            }

            return context;
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnAllProducts()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            context.Products.Add(new Product
            {
                Name = "Test Product",
                ShortDescription = "Short desc",
                LongDescription = "Long description",
                MainImageUrl = "http://example.com/img.png",
                Price = 10m,
                Stock = 5,
                CategoryId = 1
            });
            await context.SaveChangesAsync();

            var service = new ProductService(context);

            // Act
            var result = await service.GetAllAsync();

            // Assert
            Assert.Contains(result, p => p.Name == "Test Product");
        }

        [Fact]
        public async Task AddAsync_ShouldAddProductToDatabase()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var service = new ProductService(context);

            var newProduct = new Product
            {
                Name = "New Product",
                ShortDescription = "New short",
                LongDescription = "New long",
                MainImageUrl = "http://example.com/new.png",
                Price = 99m,
                Stock = 10,
                CategoryId = 1
            };

            // Act
            var created = await service.AddAsync(newProduct);

            // Assert
            var persisted = await context.Products.FindAsync(created.Id);
            Assert.NotNull(persisted);
            Assert.Equal("New Product", persisted!.Name);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnCorrectProduct()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var product = new Product
            {
                Name = "FindMe",
                ShortDescription = "Find short",
                LongDescription = "Find long",
                MainImageUrl = "http://example.com/find.png",
                Price = 100m,
                Stock = 10,
                CategoryId = 1
            };
            context.Products.Add(product);
            await context.SaveChangesAsync();

            var service = new ProductService(context);

            // Act
            var result = await service.GetByIdAsync(product.Id);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("FindMe", result!.Name);
        }

        [Fact]
        public async Task DeleteAsync_ShouldRemoveProduct()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var product = new Product
            {
                Name = "ToDelete",
                ShortDescription = "Delete short",
                LongDescription = "Delete long",
                MainImageUrl = "http://example.com/del.png",
                Price = 15m,
                Stock = 2,
                CategoryId = 1
            };
            context.Products.Add(product);
            await context.SaveChangesAsync();

            var service = new ProductService(context);

            // Act
            var success = await service.DeleteAsync(product.Id);

            // Assert
            Assert.True(success);
            var shouldNotExist = await context.Products.FindAsync(product.Id);
            Assert.Null(shouldNotExist);
        }
    }
}
