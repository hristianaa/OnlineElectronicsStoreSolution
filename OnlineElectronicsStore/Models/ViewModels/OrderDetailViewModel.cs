using System;
using System.Collections.Generic;

namespace OnlineElectronicsStore.Models.ViewModels
{
    public class OrderDetailViewModel
    {
        public int Id { get; set; }
        public DateTime CreatedOn { get; set; }
        public string Status { get; set; } = string.Empty;
        public decimal Total { get; set; }

        public List<OrderProductInfo> Items { get; set; } = new();

        public class OrderProductInfo
        {
            public string ProductName { get; set; } = string.Empty;
            public int Quantity { get; set; }
            public decimal UnitPrice { get; set; }
            public decimal LineTotal => Quantity * UnitPrice;
        }
    }
}
