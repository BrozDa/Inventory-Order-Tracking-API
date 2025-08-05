using Inventory_Order_Tracking.API.Dtos;
using Inventory_Order_Tracking.API.Services.Shared;

namespace Inventory_Order_Tracking.API.Services.Interfaces
{
    public interface IProductService
    {
        Task<ServiceResult<List<ProductAdminDto>>> AdminsGetAllAsync();

        Task<ServiceResult<ProductAdminDto>> AdminsGetSingleAsync(Guid id);

        Task<ServiceResult<List<ProductCustomerDto>>> CustomersGetAllAsync();

        Task<ServiceResult<ProductCustomerDto>> CustomersGetSingleAsync(Guid id);

        Task<ServiceResult<ProductAdminDto>> UpdateAsync(Guid id, ProductUpdateDto dto);

        Task<ServiceResult<ProductAdminDto>> UpdateNameAsync(Guid id, string newName);

        Task<ServiceResult<ProductAdminDto>> UpdateDescriptionAsync(Guid id, string newDescription);

        Task<ServiceResult<ProductAdminDto>> UpdatePriceAsync(Guid id, decimal newPrice);

        Task<ServiceResult<ProductAdminDto>> UpdateStockQuantityAsync(Guid id, int newStockQuantity);

        Task<ServiceResult<ProductAdminDto>> AddAsync(ProductAddDto dto);

        Task<ServiceResult<ProductAdminDto>> DeleteAsync(Guid id);
    }
}