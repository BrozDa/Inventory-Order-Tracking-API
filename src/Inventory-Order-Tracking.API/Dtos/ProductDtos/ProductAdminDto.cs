using Inventory_Order_Tracking.API.Domain;

namespace Inventory_Order_Tracking.API.Dtos
{
    public class ProductAdminDto
    {
        public Guid Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string? Description { get; set; }

        public int StockQuantity { get; set; }

        public decimal Price { get; set; }
    }
}
