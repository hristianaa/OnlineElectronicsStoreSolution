using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnlineElectronicsStore.Models
{
    public class Payment
    {
        public int Id { get; set; }

        // Foreign key relationship with Order
        [Required]
        public int OrderId { get; set; }

        // Navigation property for related Order
        public Order Order { get; set; }

        [Required]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal Amount { get; set; }

        [Required]
        public DateTime PaymentDate { get; set; }

        // Default Status value (e.g., Pending)
        [Required]
        [MaxLength(50)] // Limiting the length for the Status field
        public string Status { get; set; } = "Pending"; // Default to "Pending" if not set

        // This property calculates whether the payment is "Paid"
        public bool IsPaid => Status == "Paid";  // Only check "Paid" status
    }
}
