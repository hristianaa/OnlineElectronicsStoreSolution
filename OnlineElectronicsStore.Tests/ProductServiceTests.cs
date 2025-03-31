using Microsoft.EntityFrameworkCore;
using OnlineElectronicsStore.Data;
using OnlineElectronicsStore.Models;
using OnlineElectronicsStore.Services.Implementations;
using System;
using System.Linq;
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
        public void GetAllProducts_ShouldReturnAllProducts()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            context.Products.Add(new Product { Name = "Test Product", Description = "Description", Price = 10, Stock = 5 });
            context.SaveChanges();

            var service = new ProductService(context);

            // Act
            var result = service.GetAllProducts();

            // Assert
            Assert.NotEmpty(result);
        }

        [Fact]
        public void AddProduct_ShouldAddProductToDatabase()
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
            service.AddProduct(newProduct);
            var result = context.Products.FirstOrDefault(p => p.Name == "New Product");

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public void GetById_ShouldReturnCorrectProduct()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var product = new Product { Name = "FindMe", Description = "Desc", Price = 20, Stock = 3 };
            context.Products.Add(product);
            context.SaveChanges();

            var service = new ProductService(context);

            // Act
            var result = service.GetById(product.Id);

            // Assert
            Assert.Equal("FindMe", result?.Name);
        }

        [Fact]
        public void DeleteProduct_ShouldRemoveProduct()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var product = new Product { Name = "ToDelete", Description = "Gone", Price = 15, Stock = 2 };
            context.Products.Add(product);
            context.SaveChanges();

            var service = new ProductService(context);

            // Act
            service.DeleteProduct(product.Id);

            // Assert
            var result = context.Products.FirstOrDefault(p => p.Id == product.Id);
            Assert.Null(result);
        }
    }
}
