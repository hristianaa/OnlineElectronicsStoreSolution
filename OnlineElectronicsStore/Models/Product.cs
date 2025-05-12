using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnlineElectronicsStore.Models
{
    public class Product
    {
        public int Id { get; set; }

        [Required, MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        [Required, MaxLength(200)]
        [Display(Name = "Short Description")]
        public string ShortDescription { get; set; } = string.Empty;

        [DataType(DataType.MultilineText)]
        [Display(Name = "Detailed Description")]
        public string LongDescription { get; set; } = string.Empty;

        [DataType(DataType.Currency)]
        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be a positive value.")]
        public decimal Price { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Stock cannot be negative.")]
        public int Stock { get; set; }

        [Display(Name = "Main Image URL")]
        [DataType(DataType.ImageUrl)]
        public string MainImageUrl { get; set; } = string.Empty;

        public int CategoryId { get; set; }
        [ForeignKey(nameof(CategoryId))]
        public Category Category { get; set; } = null!;

        /// <summary>
        /// Additional photos for the product.
        /// </summary>
        public List<ProductPhoto> Photos { get; set; } = new();
    }

    public class ProductPhoto
    {
        public int Id { get; set; }

        [Required]
        [DataType(DataType.ImageUrl)]
        public string Url { get; set; } = string.Empty;

        public int ProductId { get; set; }
        [ForeignKey(nameof(ProductId))]
        public Product Product { get; set; } = null!;
    }
}
