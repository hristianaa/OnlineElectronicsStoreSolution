using System.ComponentModel.DataAnnotations;

namespace OnlineElectronicsStore.Models
{
    public class Product
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Product name is required.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Description is required.")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Price is required.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than zero.")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "Stock quantity is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "Stock must be at least 1.")]
        public int Stock { get; set; }

        [Required(ErrorMessage = "CategoryId is required.")]
        public int CategoryId { get; set; }   // ✅ Correct Property

        // Navigation Property (Optional for Data Relationships - Exclude in POST)
        public Category? Category { get; set; }  // Changed to nullable
    }
}
