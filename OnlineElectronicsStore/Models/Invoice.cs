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

        public Order Order { get; set; }

        [Required]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal TotalAmount { get; set; }

        [Required]
        [EmailAddress]
        public string BillingEmail { get; set; } = string.Empty;

        [Column(TypeName = "timestamp without time zone")]
        public DateTime InvoiceDate { get; set; }
    }
}
