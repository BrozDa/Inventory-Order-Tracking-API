using Inventory_Order_Tracking.API.Dtos;
using Inventory_Order_Tracking.API.Repository.Interfaces;
using Inventory_Order_Tracking.API.Services.Interfaces;
using Inventory_Order_Tracking.API.Services.Shared;

namespace Inventory_Order_Tracking.API.Services
{
    public class ProductService(
        IProductRepository repository,
        ILogger<ProductService> logger) : IProductService
    {
        public async Task<ProductServiceResult<List<ProductCustomerDto>>> CustomersGetAllAsync()
        {
            try
            {
                var fetchedEntitites = await repository.GetAllAsync();
                List<ProductCustomerDto> dtos = fetchedEntitites.Select(x => x.ToCustomerDto()).ToList();

                return ProductServiceResult<List<ProductCustomerDto>>.Ok(dtos);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "[CustomerGetAllAsync] Unhandled Exception has occured");
                return ProductServiceResult<List<ProductCustomerDto>>.InternalServerError("Failed to fetch products from database");
            }
        }
        public async Task<ProductServiceResult<ProductCustomerDto>> CustomersGetSingleAsync(Guid id)
        {
            try
            {
                var fetchedEntity = await repository.GetByIdAsync(id);
                if (fetchedEntity is null)
                    return ProductServiceResult<ProductCustomerDto>.NotFound();

                return ProductServiceResult<ProductCustomerDto>.Ok(fetchedEntity.ToCustomerDto());
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "[CustomerGetSingleAsync] Unhandled Exception has occured");
                return ProductServiceResult<ProductCustomerDto>.InternalServerError("Failed to fetch product from database");
            }
        }
        public async Task<ProductServiceResult<List<ProductAdminDto>>> AdminsGetAllAsync()
        {
            try
            {
                var fetchedEntitites = await repository.GetAllAsync();
                List<ProductAdminDto> dtos = fetchedEntitites.Select(x => x.ToAdminDto()).ToList();

                return ProductServiceResult<List<ProductAdminDto>>.Ok(dtos);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "[CustomerGetAllAsync] Unhandled Exception has occured");
                return ProductServiceResult<List<ProductAdminDto>>.InternalServerError("Failed to fetch products from database");
            }
        }
        public async Task<ProductServiceResult<ProductAdminDto>> AdminsGetSingleAsync(Guid id)
        {
            try
            {
                var fetchedEntity = await repository.GetByIdAsync(id);
                if (fetchedEntity is null)
                    return ProductServiceResult<ProductAdminDto>.NotFound();

                return ProductServiceResult<ProductAdminDto>.Ok(fetchedEntity.ToAdminDto());
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "[CustomerGetSingleAsync] Unhandled Exception has occured");
                return ProductServiceResult<ProductAdminDto>.InternalServerError("Failed to fetch product from database");
            }
        }
    }
}
