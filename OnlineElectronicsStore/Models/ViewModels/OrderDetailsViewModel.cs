// Models/ViewModels/OrderDetailsViewModel.cs
using System;
using System.Collections.Generic;

namespace OnlineElectronicsStore.Models.ViewModels
{
    public class OrderDetailsViewModel
    {
        public int Id { get; set; }

        // Razor uses Model.CreatedOn, so name it CreatedOn:
        public DateTime CreatedOn { get; set; }

        public string Status { get; set; } = string.Empty;
        public decimal Total { get; set; }

        public List<OrderItemViewModel> Items { get; set; } = new();
    }

    public class OrderItemViewModel
    {
        public string ProductName { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal LineTotal => Quantity * UnitPrice;
    }
}
