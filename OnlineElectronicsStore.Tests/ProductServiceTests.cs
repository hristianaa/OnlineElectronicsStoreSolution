using Microsoft.EntityFrameworkCore;
using OnlineElectronicsStore.Data;
using OnlineElectronicsStore.Models;
using OnlineElectronicsStore.Services;
using OnlineElectronicsStore.Services.Implementations;
using System;
using System.Linq;
using System.Threading.Tasks;  // Required for async
using Xunit;

namespace OnlineElectronicsStore.Tests
{
    public class ProductServiceTests
    {
        private AppDbContext GetInMemoryDbContext()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            var context = new AppDbContext(options);
            context.Database.EnsureCreated();

            return context;
        }

        [Fact]
        public async Task GetAllProducts_ShouldReturnAllProducts()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            context.Products.Add(new Product { Name = "Test Product", Description = "Description", Price = 10, Stock = 5 });
            await context.SaveChangesAsync();  // Make sure to await async save

            var service = new ProductService(context);

            // Act
            var result = await service.GetAllProducts();  // Await the asynchronous method

            // Assert
            Assert.NotEmpty(result);
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
            await service.AddProduct(newProduct);  // Await the asynchronous method
            var result = await context.Products.FirstOrDefaultAsync(p => p.Name == "New Product");

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public async Task GetById_ShouldReturnCorrectProduct()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var product = new Product { Name = "FindMe", Description = "Test Product", Price = 100, Stock = 10 };
            context.Products.Add(product);
            await context.SaveChangesAsync();  // Save changes asynchronously

            var service = new ProductService(context);

            // Act
            var result = await service.GetById(product.Id);  // Await the asynchronous method

            // Assert
            Assert.Equal("FindMe", result?.Name);  // Access Name after awaiting the result
        }

        [Fact]
        public async Task DeleteProduct_ShouldRemoveProduct()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var product = new Product { Name = "ToDelete", Description = "Gone", Price = 15, Stock = 2 };
            context.Products.Add(product);
            await context.SaveChangesAsync();  // Save changes asynchronously

            var service = new ProductService(context);

            // Act
            await service.DeleteProduct(product.Id);  // Await the asynchronous method

            // Assert
            var result = await context.Products.FirstOrDefaultAsync(p => p.Id == product.Id);
            Assert.Null(result);
        }
    }
}
