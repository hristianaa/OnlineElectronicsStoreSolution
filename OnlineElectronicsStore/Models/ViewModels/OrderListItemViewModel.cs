using System;

namespace OnlineElectronicsStore.Models.ViewModels
{
    public class OrderListItemViewModel
    {
        public int Id { get; set; }
        public DateTime CreatedOn { get; set; }
        public decimal Total { get; set; }
        public string Status { get; set; } = string.Empty;
    }
}
