using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnlineElectronicsStore.Models
{
    public class Product
    {
        public int Id { get; set; }

        [Required, StringLength(100)]
        public string Name { get; set; } = string.Empty;

        [Required, StringLength(200)]
        [Display(Name = "Short Description")]
        public string ShortDescription { get; set; } = string.Empty;

        [Required, StringLength(2000)]
        [DataType(DataType.MultilineText)]
        [Display(Name = "Detailed Description")]
        public string LongDescription { get; set; } = string.Empty;

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        [DataType(DataType.Currency)]
        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be positive.")]
        public decimal Price { get; set; }

        [Required]
        [Range(0, int.MaxValue, ErrorMessage = "Stock cannot be negative.")]
        public int Stock { get; set; }

        [Required, Url]
        [Display(Name = "Main Image URL")]
        public string MainImageUrl { get; set; } = string.Empty;

        // ← if you later want to store multiple photos per product
        public ICollection<ProductPhoto> Photos { get; set; } = new List<ProductPhoto>();
      
        // FK to Category
        [ForeignKey(nameof(Category))]
        [Display(Name = "Category")]
        public int CategoryId { get; set; }

        public Category Category { get; set; } = null!;
    }
}
