// Models/ViewModels/OrderHistoryViewModel.cs
using System;

namespace OnlineElectronicsStore.Models.ViewModels
{
    public class OrderHistoryViewModel
    {
        public int Id { get; set; }
        public DateTime CreatedOn { get; set; }
        public decimal Total { get; set; }
        public string Status { get; set; } = string.Empty;
    }
}
