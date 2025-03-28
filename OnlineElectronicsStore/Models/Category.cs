﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace OnlineElectronicsStore.Models
{
    public class Category
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        // Navigation Property
        public ICollection<Product> Products { get; set; }
    }
}
