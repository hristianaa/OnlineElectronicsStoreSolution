// Models/ViewModels/OrderDetailsViewModel.cs
// Models/ViewModels/OrderDetailsViewModel.cs
using System;
using System.Collections.Generic;

namespace OnlineElectronicsStore.Models.ViewModels
{
    public class OrderDetailsViewModel
    {
        public int Id { get; set; }
        public DateTime OrderDate { get; set; }
        public string Status { get; set; } = string.Empty;
        public decimal Total { get; set; }

        // ← ensure this matches exactly what the controller sets
        public List<OrderItemViewModel> Items { get; set; } = new();
    }

    public class OrderItemViewModel
    {
        // ← these three must exist to match controller projections
        public string ProductName { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }

        // optional convenience property
        public decimal LineTotal => Quantity * UnitPrice;
    }
}
