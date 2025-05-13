using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using OnlineElectronicsStore.Models;

namespace OnlineElectronicsStore.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductPhoto> ProductPhotos { get; set; }
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
        public DbSet<SupportMessage> SupportMessages { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // 1. Global mapping: DateTime → datetime2 + GETDATE()
            var dateProps = modelBuilder.Model
                .GetEntityTypes()
                .SelectMany(t => t.GetProperties())
                .Where(p => p.ClrType == typeof(DateTime) || p.ClrType == typeof(DateTime?));

            foreach (var prop in dateProps)
            {
                prop.SetColumnType("datetime2");
                if (!prop.IsNullable)
                    prop.SetDefaultValueSql("GETDATE()");
            }

            // 2. Relationships
            modelBuilder.Entity<OrderItem>()
                .HasOne(oi => oi.ParentOrder)
                .WithMany(o => o.OrderItems)
                .HasForeignKey(oi => oi.OrderId);

            modelBuilder.Entity<OrderItem>()
                .HasOne(oi => oi.Product)
                .WithMany()
                .HasForeignKey(oi => oi.ProductId);

            // 2.5 Decimal precision for money fields
            modelBuilder.Entity<Order>()
                .Property(o => o.TotalAmount)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<OrderItem>()
                .Property(oi => oi.UnitPrice)
                .HasColumnType("decimal(18,2)");

            // 3. Seed Categories
            modelBuilder.Entity<Category>().HasData(
                new Category { Id = 1, Name = "Laptops" },
                new Category { Id = 2, Name = "PC Components" },
                new Category { Id = 3, Name = "Peripherals" },
                new Category { Id = 4, Name = "Smartphones" },
                new Category { Id = 5, Name = "Audio Devices" },
                new Category { Id = 6, Name = "Gaming Accessories" }
            );

            // 4. Seed Products with descriptions + images
            modelBuilder.Entity<Product>().HasData(
                new Product
                {
                    Id = 1,
                    Name = "Gaming Laptop",
                    ShortDescription = "High-end gaming laptop with RTX graphics",
                    LongDescription = "A powerhouse machine featuring an NVIDIA RTX GPU, 16 GB RAM, and a 1 TB SSD—perfect for top-tier gaming and streaming.",
                    MainImageUrl = "/images/products/1-main.jpg",
                    Price = 1200m,
                    Stock = 15,
                    CategoryId = 1
                },
                new Product
                {
                    Id = 2,
                    Name = "Business Laptop",
                    ShortDescription = "Lightweight, portable business laptop",
                    LongDescription = "An ultra-portable device with 14\" FHD display, 8 GB RAM, and long battery life—ideal for professionals on the move.",
                    MainImageUrl = "/images/products/2-main.jpg",
                    Price = 950m,
                    Stock = 20,
                    CategoryId = 1
                },
                new Product
                {
                    Id = 3,
                    Name = "UltraBook Pro",
                    ShortDescription = "Compact and powerful ultrabook",
                    LongDescription = "Slim design with 11th Gen Intel Core, 512 GB SSD, and up to 12 hours battery life—great for travel and productivity.",
                    MainImageUrl = "/images/products/3-main.jpg",
                    Price = 1500m,
                    Stock = 10,
                    CategoryId = 1
                },
                new Product
                {
                    Id = 4,
                    Name = "Graphics Card RTX 4070",
                    ShortDescription = "Next-gen graphics card for gaming",
                    LongDescription = "Equipped with 8 GB GDDR6, DLSS support, and ray-tracing cores for ultra-realistic graphics in the latest games.",
                    MainImageUrl = "/images/products/4-main.jpg",
                    Price = 600m,
                    Stock = 25,
                    CategoryId = 2
                },
                new Product
                {
                    Id = 5,
                    Name = "Intel i7 Processor",
                    ShortDescription = "12th Gen Intel Core i7 CPU",
                    LongDescription = "Offers 8 cores and 16 threads, up to 4.9 GHz Turbo Boost, and DDR5 memory support—ideal for gaming and content creation.",
                    MainImageUrl = "/images/products/5-main.jpg",
                    Price = 400m,
                    Stock = 30,
                    CategoryId = 2
                },
                new Product
                {
                    Id = 6,
                    Name = "750W Power Supply",
                    ShortDescription = "High-efficiency ATX power supply",
                    LongDescription = "80 Plus Gold PSU with modular cables, Japanese capacitors, and silent fan for reliable, quiet performance.",
                    MainImageUrl = "/images/products/6-main.jpg",
                    Price = 100m,
                    Stock = 40,
                    CategoryId = 2
                },
                new Product
                {
                    Id = 7,
                    Name = "Mechanical Keyboard",
                    ShortDescription = "RGB mechanical gaming keyboard",
                    LongDescription = "Durable switches with customizable RGB backlighting, programmable macros, and detachable wrist rest for comfort.",
                    MainImageUrl = "/images/products/7-main.jpg",
                    Price = 80m,
                    Stock = 50,
                    CategoryId = 3
                },
                new Product
                {
                    Id = 8,
                    Name = "Wireless Mouse",
                    ShortDescription = "Ergonomic 2.4 GHz wireless mouse",
                    LongDescription = "Adjustable DPI to 16,000, contoured shape, and up to 60 hours battery life—perfect for work or play.",
                    MainImageUrl = "/images/products/8-main.jpg",
                    Price = 25m,
                    Stock = 100,
                    CategoryId = 3
                },
                new Product
                {
                    Id = 9,
                    Name = "27-inch 4K Monitor",
                    ShortDescription = "Ultra HD IPS display monitor",
                    LongDescription = "27\" 3840×2160 panel with HDR10, 95% DCI-P3, and built-in speakers—great for design, gaming, and streaming.",
                    MainImageUrl = "/images/products/9-main.jpg",
                    Price = 300m,
                    Stock = 12,
                    CategoryId = 3
                },
                new Product
                {
                    Id = 10,
                    Name = "iPhone 14 Pro",
                    ShortDescription = "Apple's flagship smartphone",
                    LongDescription = "6.1\" Super Retina XDR, A16 Bionic chip, Pro camera system with 48 MP main sensor, and Dynamic Island.",
                    MainImageUrl = "/images/products/10-main.jpg",
                    Price = 1100m,
                    Stock = 20,
                    CategoryId = 4
                },
                new Product
                {
                    Id = 11,
                    Name = "Samsung Galaxy S23",
                    ShortDescription = "Samsung flagship with Dynamic AMOLED",
                    LongDescription = "6.1\" AMOLED 2X, Snapdragon 8 Gen 2, triple-camera setup, 3900 mAh battery with 25 W charging.",
                    MainImageUrl = "/images/products/11-main.jpg",
                    Price = 1000m,
                    Stock = 18,
                    CategoryId = 4
                },
                new Product
                {
                    Id = 12,
                    Name = "Google Pixel 7",
                    ShortDescription = "Google’s AI-powered smartphone",
                    LongDescription = "6.3\" OLED, Google Tensor G2 chip, Magic Eraser, and 30 W fast charging.",
                    MainImageUrl = "/images/products/12-main.jpg",
                    Price = 850m,
                    Stock = 15,
                    CategoryId = 4
                },
                new Product
                {
                    Id = 13,
                    Name = "Sony WH-1000XM5",
                    ShortDescription = "Premium noise-cancelling headphones",
                    LongDescription = "Industry-leading ANC, 30 h battery life, multi-device pairing, and hi-res audio support.",
                    MainImageUrl = "/images/products/13-main.jpg",
                    Price = 300m,
                    Stock = 22,
                    CategoryId = 5
                },
                new Product
                {
                    Id = 14,
                    Name = "JBL Bluetooth Speaker",
                    ShortDescription = "Portable waterproof speaker",
                    LongDescription = "JBL Signature Sound, 12 h playtime, and IPX7 water resistance—ideal for outdoors.",
                    MainImageUrl = "/images/products/14-main.jpg",
                    Price = 150m,
                    Stock = 35,
                    CategoryId = 5
                },
                new Product
                {
                    Id = 15,
                    Name = "Gaming Headset",
                    ShortDescription = "Over-ear RGB gaming headset",
                    LongDescription = "7.1 virtual surround, noise-cancelling mic, memory foam earcups, USB/3.5 mm connectivity.",
                    MainImageUrl = "/images/products/15-main.jpg",
                    Price = 70m,
                    Stock = 40,
                    CategoryId = 6
                },
                new Product
                {
                    Id = 16,
                    Name = "Racing Wheel Controller",
                    ShortDescription = "Force-feedback racing wheel",
                    LongDescription = "Realistic force feedback, pedal set with clutch, and adjustable angle—perfect for sims.",
                    MainImageUrl = "/images/products/16-main.jpg",
                    Price = 250m,
                    Stock = 5,
                    CategoryId = 6
                },
                new Product
                {
                    Id = 17,
                    Name = "RGB Mousepad",
                    ShortDescription = "Customizable RGB mousepad for gamers",
                    LongDescription = "Dynamic lighting, non-slip rubber bottom, and smooth surface for precise control.",
                    MainImageUrl = "/images/products/17-main.jpg",
                    Price = 30m,
                    Stock = 80,
                    CategoryId = 6
                }
            );

            // 5. Seed Discounts
            modelBuilder.Entity<Discount>().HasData(
                new Discount
                {
                    Id = 1,
                    DiscountCode = "WELCOME10",
                    DiscountAmount = 10.00M,
                    ExpiryDate = new DateTime(2025, 12, 31),
                    ProductId = 1
                },
                new Discount
                {
                    Id = 2,
                    DiscountCode = "SUMMER20",
                    DiscountAmount = 20.00M,
                    ExpiryDate = new DateTime(2025, 6, 30),
                    ProductId = 3
                }
            );

            // 6. Seed Users
            modelBuilder.Entity<User>().HasData(
                new User
                {
                    Id = 1,
                    FullName = "Admin User",
                    Email = "admin@example.com",
                    Password = "Admin@123",
                    Role = "Admin"
                },
                new User
                {
                    Id = 2,
                    FullName = "John Doe",
                    Email = "john.doe@example.com",
                    Password = "User@123",
                    Role = "User"
                }
            );
        }
    }
}

