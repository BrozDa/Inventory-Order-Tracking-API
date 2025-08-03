using Inventory_Order_Tracking.API.Dtos;
using Inventory_Order_Tracking.API.Services.Shared;

namespace Inventory_Order_Tracking.API.Services.Interfaces
{
    public interface IProductService
    {
        Task<ProductServiceResult<List<ProductAdminDto>>> AdminsGetAllAsync();
        Task<ProductServiceResult<ProductAdminDto>> AdminsGetSingleAsync(Guid id);
        Task<ProductServiceResult<List<ProductCustomerDto>>> CustomersGetAllAsync();
        Task<ProductServiceResult<ProductCustomerDto>> CustomersGetSingleAsync(Guid id);
        Task<ProductServiceResult<ProductAdminDto>> UpdateAsync(Guid id, ProductUpdateDto dto);
        Task<ProductServiceResult<ProductAdminDto>> UpdateNameAsync(Guid id, string newName);
        Task<ProductServiceResult<ProductAdminDto>> UpdateDescriptionAsync(Guid id, string newDescription);
        Task<ProductServiceResult<ProductAdminDto>> UpdatePriceAsync(Guid id, decimal newPrice);
        Task<ProductServiceResult<ProductAdminDto>> UpdateStockQuantityAsync(Guid id, int newStockQuantity);
        Task<ProductServiceResult<ProductAdminDto>> AddAsync(ProductAddDto dto);
        Task<ProductServiceResult<ProductAdminDto>> DeleteAsync(Guid id);
    }
}