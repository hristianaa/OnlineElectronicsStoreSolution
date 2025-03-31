using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnlineElectronicsStore.Models
{
    public class Invoice
    {
        public int Id { get; set; }

        [Required]
        public int OrderId { get; set; }

        // 🔗 Navigation property
        public Order Order { get; set; }

        [Required]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal TotalAmount { get; set; }

        // 📧 Optional billing email for recordkeeping or guest orders
        [Required]
        [EmailAddress]
        public string BillingEmail { get; set; } = string.Empty;

        // 📅 Auto timestamp when generated
        public DateTime InvoiceDate { get; set; } = DateTime.UtcNow;
    }
}
