using System.ComponentModel.DataAnnotations;

namespace OnlineElectronicsStore.Models
{
    public class CartItem
    {
        public int Id { get; set; }

        [Required]
        public int ProductId { get; set; }
        public Product Product { get; set; }

        [Required]
        public int UserId { get; set; }
        public User User { get; set; }

        [Required]
        public int Quantity { get; set; }
    }
}
