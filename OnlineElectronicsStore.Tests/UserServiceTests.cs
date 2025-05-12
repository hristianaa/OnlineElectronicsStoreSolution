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
    public class UserServiceTests
    {
        private AppDbContext GetInMemoryDbContext()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var context = new AppDbContext(options);
            context.Database.EnsureCreated();

            // Seed Users
            if (!context.Users.Any())
            {
                context.Users.AddRange(
                    new User { Id = 1, FullName = "Admin", Email = "admin@example.com", Password = "Admin@123", Role = "Admin" },
                    new User { Id = 2, FullName = "User", Email = "user@example.com", Password = "User@123", Role = "User" }
                );
                context.SaveChanges();
            }

            return context;
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnTwoUsers()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var service = new UserService(context);

            // Act
            var users = await service.GetAllAsync();

            // Assert
            Assert.Equal(2, users.Count());
        }

        [Fact]
        public async Task DeleteAsync_ShouldRemoveUser()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var service = new UserService(context);

            // Act
            var success = await service.DeleteAsync(2);

            // Assert
            Assert.True(success);
            Assert.Single(context.Users);
            Assert.DoesNotContain(context.Users, u => u.Id == 2);
        }
    }
}
