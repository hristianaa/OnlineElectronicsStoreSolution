using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnlineElectronicsStore.Models
{
    public class Discount
    {
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string DiscountCode { get; set; } = string.Empty;

        [Required]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal DiscountAmount { get; set; }

        // 🛡️ Set default as UtcNow + 30 days to ensure it's UTC
        [Required]
        public DateTime ExpiryDate { get; set; } = DateTime.SpecifyKind(DateTime.UtcNow.AddDays(30), DateTimeKind.Utc);

        // 🔗 Foreign Key
        [Required]
        public int ProductId { get; set; }

        public Product Product { get; set; } = null!;
    }
}
