namespace Inventory_Order_Tracking.API.Dtos
{
    public class ProductAddDto
    {
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public int? StockQuantity { get; set; }
    }
}