// Models/ViewModels/OrderListItemViewModel.cs
using System;

namespace OnlineElectronicsStore.Models.ViewModels
{
    public class OrderListItemViewModel
    {
        /// <summary>
        /// The primary key of the order
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// When the order was placed
        /// </summary>
        public DateTime CreatedOn { get; set; }

        /// <summary>
        /// Total cost of the order
        /// </summary>
        public decimal Total { get; set; }

        /// <summary>
        /// Current status (e.g. Pending, Shipped, Completed, Cancelled)
        /// </summary>
        public string Status { get; set; } = string.Empty;
    }
}
