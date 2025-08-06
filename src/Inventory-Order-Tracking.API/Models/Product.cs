using Inventory_Order_Tracking.API.Domain;
using Inventory_Order_Tracking.API.Dtos;

namespace Inventory_Order_Tracking.API.Models
{

    /// <summary>
    /// Represents a product which can be shown or placed in the order.
    /// </summary>
    public class Product
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public int StockQuantity { get; set; }
        public decimal Price { get; set; }
        public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();

        /// <summary>
        /// Maps an existing <see cref="Product"/> to <see cref="ProductCustomerDto"/>
        /// </summary>
        /// <returns>An instance <see cref="ProductCustomerDto"/> with mapped values</returns>
        public ProductCustomerDto ToCustomerDto()
        {
            var dto = new ProductCustomerDto()
            {
                Id = Id,
                Name = Name,
                Description = Description,
                Price = Price,
            };

            dto.StockStatus = StockQuantity switch
            {
                < 0 => StockStatus.Unavailable,
                < 5 => StockStatus.Low,
                _ => StockStatus.Available
            };

            return dto;
        }

        /// <summary>
        /// Maps an existing <see cref="Product"/> to <see cref="ProductAdminDto"/>
        /// </summary>
        /// <returns>An instance <see cref="ProductAdminDto"/> with mapped values</returns>
        public ProductAdminDto ToAdminDto()
        {
            return new ProductAdminDto()
            {
                Id = Id,
                Name = Name,
                Description = Description,
                Price = Price,
                StockQuantity = StockQuantity
            };
        }
    }
}