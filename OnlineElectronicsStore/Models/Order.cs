using System;
using System.ComponentModel.DataAnnotations;

namespace OnlineElectronicsStore.Models
{
    public class Order
    {
        public int Id { get; set; }

        [Required]
        public int UserId { get; set; }
        public User User { get; set; }

        [Required]
        public DateTime OrderDate { get; set; }

        [Required]
        public string Status { get; set; }

        // Navigation Property
        public ICollection<CartItem> CartItems { get; set; }
        
        public decimal TotalAmount { get; set; }

        public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
    }
}
