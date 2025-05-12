using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OnlineElectronicsStore.Migrations
{
    /// <inheritdoc />
    public partial class AddDecimalPrecisionToOrders : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "Products");

            migrationBuilder.AddColumn<string>(
                name: "LongDescription",
                table: "Products",
                type: "nvarchar(2000)",
                maxLength: 2000,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "MainImageUrl",
                table: "Products",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ShortDescription",
                table: "Products",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "ProductPhotos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Url = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ProductId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductPhotos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductPhotos_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "LongDescription", "MainImageUrl", "ShortDescription" },
                values: new object[] { "A powerhouse machine featuring an NVIDIA RTX GPU, 16 GB RAM, and a 1 TB SSD—perfect for top-tier gaming and streaming.", "/images/products/1-main.jpg", "High-end gaming laptop with RTX graphics" });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "LongDescription", "MainImageUrl", "ShortDescription" },
                values: new object[] { "An ultra-portable device with 14\" FHD display, 8 GB RAM, and long battery life—ideal for professionals on the move.", "/images/products/2-main.jpg", "Lightweight, portable business laptop" });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "LongDescription", "MainImageUrl", "ShortDescription" },
                values: new object[] { "Slim design with 11th Gen Intel Core, 512 GB SSD, and up to 12 hours battery life—great for travel and productivity.", "/images/products/3-main.jpg", "Compact and powerful ultrabook" });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "LongDescription", "MainImageUrl", "ShortDescription" },
                values: new object[] { "Equipped with 8 GB GDDR6, DLSS support, and ray-tracing cores for ultra-realistic graphics in the latest games.", "/images/products/4-main.jpg", "Next-gen graphics card for gaming" });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "LongDescription", "MainImageUrl", "ShortDescription" },
                values: new object[] { "Offers 8 cores and 16 threads, up to 4.9 GHz Turbo Boost, and DDR5 memory support—ideal for gaming and content creation.", "/images/products/5-main.jpg", "12th Gen Intel Core i7 CPU" });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "LongDescription", "MainImageUrl", "ShortDescription" },
                values: new object[] { "80 Plus Gold PSU with modular cables, Japanese capacitors, and silent fan for reliable, quiet performance.", "/images/products/6-main.jpg", "High-efficiency ATX power supply" });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "LongDescription", "MainImageUrl", "ShortDescription" },
                values: new object[] { "Durable switches with customizable RGB backlighting, programmable macros, and detachable wrist rest for comfort.", "/images/products/7-main.jpg", "RGB mechanical gaming keyboard" });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "LongDescription", "MainImageUrl", "ShortDescription" },
                values: new object[] { "Adjustable DPI to 16,000, contoured shape, and up to 60 hours battery life—perfect for work or play.", "/images/products/8-main.jpg", "Ergonomic 2.4 GHz wireless mouse" });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 9,
                columns: new[] { "LongDescription", "MainImageUrl", "ShortDescription" },
                values: new object[] { "27\" 3840×2160 panel with HDR10, 95% DCI-P3, and built-in speakers—great for design, gaming, and streaming.", "/images/products/9-main.jpg", "Ultra HD IPS display monitor" });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 10,
                columns: new[] { "LongDescription", "MainImageUrl", "ShortDescription" },
                values: new object[] { "6.1\" Super Retina XDR, A16 Bionic chip, Pro camera system with 48 MP main sensor, and Dynamic Island.", "/images/products/10-main.jpg", "Apple's flagship smartphone" });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 11,
                columns: new[] { "LongDescription", "MainImageUrl", "ShortDescription" },
                values: new object[] { "6.1\" AMOLED 2X, Snapdragon 8 Gen 2, triple-camera setup, 3900 mAh battery with 25 W charging.", "/images/products/11-main.jpg", "Samsung flagship with Dynamic AMOLED" });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 12,
                columns: new[] { "LongDescription", "MainImageUrl", "ShortDescription" },
                values: new object[] { "6.3\" OLED, Google Tensor G2 chip, Magic Eraser, and 30 W fast charging.", "/images/products/12-main.jpg", "Google’s AI-powered smartphone" });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 13,
                columns: new[] { "LongDescription", "MainImageUrl", "ShortDescription" },
                values: new object[] { "Industry-leading ANC, 30 h battery life, multi-device pairing, and hi-res audio support.", "/images/products/13-main.jpg", "Premium noise-cancelling headphones" });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 14,
                columns: new[] { "LongDescription", "MainImageUrl", "ShortDescription" },
                values: new object[] { "JBL Signature Sound, 12 h playtime, and IPX7 water resistance—ideal for outdoors.", "/images/products/14-main.jpg", "Portable waterproof speaker" });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 15,
                columns: new[] { "LongDescription", "MainImageUrl", "ShortDescription" },
                values: new object[] { "7.1 virtual surround, noise-cancelling mic, memory foam earcups, USB/3.5 mm connectivity.", "/images/products/15-main.jpg", "Over-ear RGB gaming headset" });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 16,
                columns: new[] { "LongDescription", "MainImageUrl", "ShortDescription" },
                values: new object[] { "Realistic force feedback, pedal set with clutch, and adjustable angle—perfect for sims.", "/images/products/16-main.jpg", "Force-feedback racing wheel" });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 17,
                columns: new[] { "LongDescription", "MainImageUrl", "ShortDescription" },
                values: new object[] { "Dynamic lighting, non-slip rubber bottom, and smooth surface for precise control.", "/images/products/17-main.jpg", "Customizable RGB mousepad for gamers" });

            migrationBuilder.CreateIndex(
                name: "IX_ProductPhotos_ProductId",
                table: "ProductPhotos",
                column: "ProductId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProductPhotos");

            migrationBuilder.DropColumn(
                name: "LongDescription",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "MainImageUrl",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "ShortDescription",
                table: "Products");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Products",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1,
                column: "Description",
                value: "Powerful laptop designed for high-performance gaming.");

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 2,
                column: "Description",
                value: "Lightweight laptop designed for business professionals.");

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 3,
                column: "Description",
                value: "Compact and powerful ultrabook for travel and work.");

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 4,
                column: "Description",
                value: "High-performance graphics card for gaming and rendering.");

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 5,
                column: "Description",
                value: "12th Gen Intel i7 processor for enhanced performance.");

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 6,
                column: "Description",
                value: "Reliable power supply unit for high-end gaming PCs.");

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 7,
                column: "Description",
                value: "Durable mechanical keyboard with RGB lighting.");

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 8,
                column: "Description",
                value: "Ergonomic wireless mouse with precision tracking.");

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 9,
                column: "Description",
                value: "High-resolution monitor perfect for design and gaming.");

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 10,
                column: "Description",
                value: "Latest iPhone with improved camera and performance.");

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 11,
                column: "Description",
                value: "Samsung flagship smartphone with enhanced display.");

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 12,
                column: "Description",
                value: "Google's premium smartphone with superior AI features.");

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 13,
                column: "Description",
                value: "Noise-cancelling wireless headphones with premium sound.");

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 14,
                column: "Description",
                value: "Portable Bluetooth speaker with powerful bass.");

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 15,
                column: "Description",
                value: "Immersive gaming headset with surround sound.");

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 16,
                column: "Description",
                value: "Realistic racing wheel for enhanced driving simulation.");

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 17,
                column: "Description",
                value: "Customizable RGB mousepad for gamers.");
        }
    }
}
