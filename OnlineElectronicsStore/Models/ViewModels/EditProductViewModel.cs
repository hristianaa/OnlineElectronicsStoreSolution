using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using OnlineElectronicsStore.Models;

namespace OnlineElectronicsStore.Models.ViewModels
{
    public class EditProductViewModel
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Short Description")]
        public string ShortDescription { get; set; } = string.Empty;

        [Display(Name = "Long Description")]
        public string? LongDescription { get; set; }

        [Range(0, double.MaxValue)]
        public decimal Price { get; set; }

        [Range(0, int.MaxValue)]
        public int Stock { get; set; }

        [Display(Name = "Category")]
        public int CategoryId { get; set; }

        // Existing main image URL for display
        public string? MainImageUrl { get; set; }

        // Existing extra photos for display
        public List<ProductPhoto> Photos { get; set; } = new();

        // New uploads
        [Display(Name = "Main Image")]
        public IFormFile? MainImageFile { get; set; }

        [Display(Name = "Additional Photos")]
        public IFormFileCollection? PhotoFiles { get; set; }
    }
}
