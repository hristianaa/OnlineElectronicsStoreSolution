// DTOs/CartDto.cs
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace OnlineElectronicsStore.DTOs
{
    public class CartDto
    {
        public List<CartLineDto> Items { get; set; } = new();
        public decimal TotalPrice => Items.Sum(i => i.Quantity * i.UnitPrice);

        
    }
}
