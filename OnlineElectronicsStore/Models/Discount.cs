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
        public string DiscountCode { get; set; }

        [Required]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal DiscountAmount { get; set; }

        [Required]
        public DateTime ExpiryDate { get; set; }

        [Required] // Required field
        public int ProductId { get; set; }
        public Product Product { get; set; }
    }
}
