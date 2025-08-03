using Inventory_Order_Tracking.API.Dtos;
using Inventory_Order_Tracking.API.Repository.Interfaces;
using Inventory_Order_Tracking.API.Services.Interfaces;
using Inventory_Order_Tracking.API.Services.Shared;
using Inventory_Order_Tracking.API.Validators;

namespace Inventory_Order_Tracking.API.Services
{
    public class ProductService(
        IProductRepository repository,
        ILogger<ProductService> logger
        ) : IProductService
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

        public async Task<ProductServiceResult<ProductAdminDto>> UpdateNameAsync(Guid id, string newName)
        {
            var entity = await repository.GetByIdAsync(id);
            if (entity is null)
            {
                logger.LogWarning("[ProductService][UpdateNameAsync] Attempted name change for non-existing product");
                return ProductServiceResult<ProductAdminDto>.NotFound();
            }
                

            entity.Name = newName;

            await repository.SaveChangesAsync();

            return ProductServiceResult<ProductAdminDto>.Ok(entity.ToAdminDto());
        }
        public async Task<ProductServiceResult<ProductAdminDto>> UpdateDescriptionAsync(Guid id, string newDescription)
        {
            var entity = await repository.GetByIdAsync(id);
            if (entity is null)
            {
                logger.LogWarning("[ProductService][UpdateDescriptionAsync] Attempted description change for non-existing product");
                return ProductServiceResult<ProductAdminDto>.NotFound();
            }

            entity.Description = newDescription;

            await repository.SaveChangesAsync();

            return ProductServiceResult<ProductAdminDto>.Ok(entity.ToAdminDto());
        }
        public async Task<ProductServiceResult<ProductAdminDto>> UpdatePriceAsync(Guid id, decimal newPrice)
        {
            var entity = await repository.GetByIdAsync(id);
            if (entity is null)
            {
                logger.LogWarning("[ProductService][UpdatePriceAsync] Attempted price change for non-existing product");
                return ProductServiceResult<ProductAdminDto>.NotFound();
            }

            entity.Price = newPrice;

            await repository.SaveChangesAsync();

            return ProductServiceResult<ProductAdminDto>.Ok(entity.ToAdminDto());
        }
        public async Task<ProductServiceResult<ProductAdminDto>> UpdateStockQuantityAsync(Guid id, int newStockQuantity)
        {
            var entity = await repository.GetByIdAsync(id);
            if (entity is null)
            {
                logger.LogWarning("[ProductService][UpdateStockQuantityAsync] Attempted stock change for non-existing product");
                return ProductServiceResult<ProductAdminDto>.NotFound();
            }

            entity.StockQuantity = newStockQuantity;

            await repository.SaveChangesAsync();

            return ProductServiceResult<ProductAdminDto>.Ok(entity.ToAdminDto());
        }
    }
}
