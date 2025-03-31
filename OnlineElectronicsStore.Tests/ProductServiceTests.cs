using Microsoft.EntityFrameworkCore;
using OnlineElectronicsStore.Data;
using OnlineElectronicsStore.Models;
using OnlineElectronicsStore.Services;
using OnlineElectronicsStore.Services.Implementations;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace OnlineElectronicsStore.Tests
{
    public class ProductServiceTests
    {
        // 🔧 Creates an isolated in-memory DB for each test
        private AppDbContext GetInMemoryDbContext()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()) // Unique per test
                .Options;

            var context = new AppDbContext(options);
            context.Database.EnsureCreated();

            // ✅ Seed Category if needed (avoid duplication)
            if (!context.Categories.Any())
            {
                context.Categories.Add(new Category { Id = 1, Name = "Test Category" });
                context.SaveChanges();
            }

            return context;
        }

        [Fact]
        public async Task GetAllProducts_ShouldReturnAllProducts()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            context.Products.Add(new Product
            {
                Name = "Test Product",
                Description = "Description",
                Price = 10,
                Stock = 5,
                CategoryId = 1
            });
            await context.SaveChangesAsync();

            var service = new ProductService(context);

            // Act
            var result = await service.GetAllProducts();

            // Assert (allowing seeded data, but checking for our item)
            Assert.Contains(result, p => p.Name == "Test Product");
        }

        [Fact]
        public async Task AddProduct_ShouldAddProductToDatabase()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var service = new ProductService(context);

            var newProduct = new Product
            {
                Name = "New Product",
                Description = "Test",
                Price = 99,
                Stock = 10,
                CategoryId = 1
            };

            // Act
            await service.AddProduct(newProduct);
            var result = await context.Products.FirstOrDefaultAsync(p => p.Name == "New Product");

            // Assert
            Assert.NotNull(result);
            Assert.Equal("New Product", result?.Name);
        }

        [Fact]
        public async Task GetById_ShouldReturnCorrectProduct()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var product = new Product
            {
                Name = "FindMe",
                Description = "Test Product",
                Price = 100,
                Stock = 10,
                CategoryId = 1
            };
            context.Products.Add(product);
            await context.SaveChangesAsync();

            var service = new ProductService(context);

            // Act
            var result = await service.GetById(product.Id);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("FindMe", result?.Name);
        }

        [Fact]
        public async Task DeleteProduct_ShouldRemoveProduct()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var product = new Product
            {
                Name = "ToDelete",
                Description = "Gone",
                Price = 15,
                Stock = 2,
                CategoryId = 1
            };
            context.Products.Add(product);
            await context.SaveChangesAsync();

            var service = new ProductService(context);

            // Act
            await service.DeleteProduct(product.Id);

            // Assert
            var result = await context.Products.FirstOrDefaultAsync(p => p.Id == product.Id);
            Assert.Null(result);
        }
    }
}

