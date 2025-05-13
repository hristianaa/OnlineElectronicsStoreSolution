namespace OnlineElectronicsStore.DTOs
{
    public class CartItemDto
    {
        public int ProductId { get; set; }
        public string? ProductName { get; set; }  // optional
        public decimal UnitPrice { get; set; }  // optional
        public int Quantity { get; set; }
    }
}