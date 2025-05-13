// DTOs/OrderDto.cs
namespace OnlineElectronicsStore.DTOs
{
    public class OrderDto
    {
        public int Id { get; set; }
        public DateTime OrderDate { get; set; }
        public string Status { get; set; } = "";
        public List<OrderLineDto> Items { get; set; } = new();
        public decimal Total => Items.Sum(i => i.Quantity * i.UnitPrice);
    }

    public class OrderLineDto
    {
        public string ProductName { get; set; } = "";
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
    }
}
