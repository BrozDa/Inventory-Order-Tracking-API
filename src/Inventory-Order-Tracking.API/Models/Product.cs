using Inventory_Order_Tracking.API.Domain;
using Inventory_Order_Tracking.API.Dtos;
using System.Runtime.CompilerServices;

namespace Inventory_Order_Tracking.API.Models
{
    public class Product
    {
        public Guid Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string? Description { get; set; }

        public int StockQuantity { get; set; }

        public decimal Price { get; set; }


        public ProductDto ToDto()
        {
            var dto = new ProductDto()
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
