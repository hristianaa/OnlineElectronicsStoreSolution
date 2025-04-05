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

        // ✅ Removed default DateTime assignment
        [Required]
        public DateTime ExpiryDate { get; set; }

        [Required]
        public int ProductId { get; set; }

        public Product Product { get; set; } = null!;
    }
}
