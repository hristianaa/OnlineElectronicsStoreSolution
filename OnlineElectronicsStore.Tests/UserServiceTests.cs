using Microsoft.EntityFrameworkCore;
using OnlineElectronicsStore.Data;
using OnlineElectronicsStore.Models;
using OnlineElectronicsStore.Services.Implementations;
using System;
using System.Linq;
using Xunit;

namespace OnlineElectronicsStore.Tests
{
    public class UserServiceTests
    {
        private AppDbContext GetInMemoryDbContext()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            var context = new AppDbContext(options);
            context.Database.EnsureCreated();

            // Seed Users
            if (!context.Users.Any())
            {
                context.Users.AddRange(
                    new User { Id = 1, Email = "admin@example.com", Password = "Admin@123", FullName = "Admin", Role = "Admin" },
                    new User { Id = 2, Email = "user@example.com", Password = "User@123", FullName = "User", Role = "User" }
                );
                context.SaveChanges();
            }

            return context;
        }

        [Fact]
        public void GetAll_ShouldReturnUsers()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var service = new UserService(context);

            // Act
            var users = service.GetAll();

            // Assert
            Assert.Equal(2, users.Count());
        }

        [Fact]
        public void Delete_ShouldRemoveUser()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var service = new UserService(context);

            // Act
            service.Delete(2); // Remove the "User"

            // Assert
            Assert.Single(context.Users); // Only "Admin" should remain
            Assert.DoesNotContain(context.Users, u => u.Id == 2);
        }
    }
}
