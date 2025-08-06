namespace Inventory_Order_Tracking.API.Dtos
{
    /// <summary>
    /// Represents the data transfer object for fetching product by admin user.
    /// Differs from CustomerDto in exact representation of stock number
    /// </summary>
    public class ProductAdminDto
    {
        public Guid Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string? Description { get; set; }

        public int StockQuantity { get; set; }

        public decimal Price { get; set; }
    }
}