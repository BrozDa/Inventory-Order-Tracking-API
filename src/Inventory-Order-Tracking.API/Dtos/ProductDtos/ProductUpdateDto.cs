namespace Inventory_Order_Tracking.API.Dtos
{
    /// <summary>
    /// Represents the data transfer object used for updating more values of a product.
    /// </summary>
    public class ProductUpdateDto
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public decimal? Price { get; set; }
        public int? StockQuantity { get; set; }
    }
}