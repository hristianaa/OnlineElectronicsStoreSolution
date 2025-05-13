// DTOs/CartLineDto.cs
namespace OnlineElectronicsStore.DTOs
{
    public class CartLineDto
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; } = "";
        public decimal UnitPrice { get; set; }
        public int Quantity { get; set; }
    }
}
