using Microsoft.EntityFrameworkCore;
using OnlineElectronicsStore.Models;
using System;
using Microsoft.AspNetCore.Identity;

namespace OnlineElectronicsStore.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<CartItem> CartItems { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<Wishlist> Wishlists { get; set; }
        public DbSet<ShippingDetails> ShippingDetails { get; set; }
        public DbSet<Discount> Discounts { get; set; }
        public DbSet<Invoice> Invoices { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var hasher = new Microsoft.AspNetCore.Identity.PasswordHasher<User>();
            // 📦 Category Seeding
            modelBuilder.Entity<Category>().HasData(
                new Category { Id = 1, Name = "Laptops" },
                new Category { Id = 2, Name = "PC Components" },
                new Category { Id = 3, Name = "Peripherals" },
                new Category { Id = 4, Name = "Smartphones" },
                new Category { Id = 5, Name = "Audio Devices" },
                new Category { Id = 6, Name = "Gaming Accessories" }
            );

            // 🖥️ Product Seeding
            modelBuilder.Entity<Product>().HasData(
                new Product { Id = 1, Name = "Gaming Laptop", Description = "Powerful laptop designed for high-performance gaming.", Price = 1200, Stock = 15, CategoryId = 1 },
                new Product { Id = 2, Name = "Business Laptop", Description = "Lightweight laptop designed for business professionals.", Price = 950, Stock = 20, CategoryId = 1 },
                new Product { Id = 3, Name = "UltraBook Pro", Description = "Compact and powerful ultrabook for travel and work.", Price = 1500, Stock = 10, CategoryId = 1 },
                new Product { Id = 4, Name = "Graphics Card RTX 4070", Description = "High-performance graphics card for gaming and rendering.", Price = 600, Stock = 25, CategoryId = 2 },
                new Product { Id = 5, Name = "Intel i7 Processor", Description = "12th Gen Intel i7 processor for enhanced performance.", Price = 400, Stock = 30, CategoryId = 2 },
                new Product { Id = 6, Name = "750W Power Supply", Description = "Reliable power supply unit for high-end gaming PCs.", Price = 100, Stock = 40, CategoryId = 2 },
                new Product { Id = 7, Name = "Mechanical Keyboard", Description = "Durable mechanical keyboard with RGB lighting.", Price = 80, Stock = 50, CategoryId = 3 },
                new Product { Id = 8, Name = "Wireless Mouse", Description = "Ergonomic wireless mouse with precision tracking.", Price = 25, Stock = 100, CategoryId = 3 },
                new Product { Id = 9, Name = "27-inch 4K Monitor", Description = "High-resolution monitor perfect for design and gaming.", Price = 300, Stock = 12, CategoryId = 3 },
                new Product { Id = 10, Name = "iPhone 14 Pro", Description = "Latest iPhone with improved camera and performance.", Price = 1100, Stock = 20, CategoryId = 4 },
                new Product { Id = 11, Name = "Samsung Galaxy S23", Description = "Samsung flagship smartphone with enhanced display.", Price = 1000, Stock = 18, CategoryId = 4 },
                new Product { Id = 12, Name = "Google Pixel 7", Description = "Google's premium smartphone with superior AI features.", Price = 850, Stock = 15, CategoryId = 4 },
                new Product { Id = 13, Name = "Sony WH-1000XM5", Description = "Noise-cancelling wireless headphones with premium sound.", Price = 300, Stock = 22, CategoryId = 5 },
                new Product { Id = 14, Name = "JBL Bluetooth Speaker", Description = "Portable Bluetooth speaker with powerful bass.", Price = 150, Stock = 35, CategoryId = 5 },
                new Product { Id = 15, Name = "Gaming Headset", Description = "Immersive gaming headset with surround sound.", Price = 70, Stock = 40, CategoryId = 6 },
                new Product { Id = 16, Name = "Racing Wheel Controller", Description = "Realistic racing wheel for enhanced driving simulation.", Price = 250, Stock = 5, CategoryId = 6 },
                new Product { Id = 17, Name = "RGB Mousepad", Description = "Customizable RGB mousepad for gamers.", Price = 30, Stock = 80, CategoryId = 6 }
            );

            // 🎁 Discounts (safe DateTime)
            modelBuilder.Entity<Discount>()
                .Property(d => d.ExpiryDate)
                .HasColumnType("timestamp without time zone");

            modelBuilder.Entity<Discount>().HasData(
                new Discount
                {
                    Id = 1,
                    DiscountCode = "WELCOME10",
                    DiscountAmount = 10.00M,
                    ExpiryDate = DateTime.SpecifyKind(new DateTime(2025, 12, 30, 0, 0, 0), DateTimeKind.Unspecified),
                    ProductId = 1
                },
                new Discount
                {
                    Id = 2,
                    DiscountCode = "SUMMER20",
                    DiscountAmount = 20.00M,
                    ExpiryDate = DateTime.SpecifyKind(new DateTime(2025, 6, 30, 0, 0, 0), DateTimeKind.Unspecified),
                    ProductId = 3
                }
            );

            // 👥 Users
            modelBuilder.Entity<User>().HasData(
                new User
                {
                    Id = 1,
                    FullName = "Admin User",
                    Email = "admin@example.com",
                    Password = hasher.HashPassword(null, "Admin@123"), // 🔥 Now hashed!
                    Role = "Admin"
                },
                new User
                {
                    Id = 2,
                    FullName = "John Doe",
                    Email = "john.doe@example.com",
                    Password = hasher.HashPassword(null, "User@123"), // 🔥 Now hashed!
                    Role = "User"
                }
            );

            // 🔁 Relationships
            modelBuilder.Entity<OrderItem>()
                .HasOne(oi => oi.ParentOrder)
                .WithMany(o => o.OrderItems)
                .HasForeignKey(oi => oi.OrderId);

            modelBuilder.Entity<OrderItem>()
                .HasOne(oi => oi.Product)
                .WithMany()
                .HasForeignKey(oi => oi.ProductId);

            // 🕓 Date column setup
            modelBuilder.Entity<Order>()
                .Property(o => o.OrderDate)
                .HasColumnType("timestamp without time zone")
                .HasDefaultValueSql("NOW()");

            modelBuilder.Entity<Payment>()
                .Property(p => p.PaymentDate)
                .HasColumnType("timestamp without time zone")
                .HasDefaultValueSql("NOW()");

            modelBuilder.Entity<Invoice>()
                .Property(i => i.InvoiceDate)
                .HasColumnType("timestamp without time zone")
                .HasDefaultValueSql("NOW()");
        }
    }
}
