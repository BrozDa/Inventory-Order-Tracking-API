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
    }
}