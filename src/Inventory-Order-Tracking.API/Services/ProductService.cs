using Inventory_Order_Tracking.API.Dtos;
using Inventory_Order_Tracking.API.Models;
using Inventory_Order_Tracking.API.Repository.Interfaces;
using Inventory_Order_Tracking.API.Services.Interfaces;
using Inventory_Order_Tracking.API.Services.Shared;

namespace Inventory_Order_Tracking.API.Services
{
    /// <summary>
    /// Provides operations related to handling and managing products.
    /// </summary>
    public class ProductService(
        ICurrentUserService currentUserService,
        IAuditLogService auditService,
        IProductRepository repository,
        ILogger<ProductService> logger
        ) : IProductService
    {
        /// <inheritdoc/>
        public async Task<ServiceResult<List<ProductCustomerDto>>> CustomersGetAllAsync()
        {
            try
            {
                var entitites = await repository.GetAllAsync();

                return ServiceResult<List<ProductCustomerDto>>.Success(
                    data: entitites.Select(x => x.ToCustomerDto()).ToList());
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "[ProductService][CustomerGetAllAsync] Unhandled Exception has occured");
                return ServiceResult<List<ProductCustomerDto>>.Failure(
                    errors: ["Failed to fetch products from database"],
                    statusCode: 500);
            }
        }

        /// <inheritdoc/>
        public async Task<ServiceResult<ProductCustomerDto>> CustomersGetSingleAsync(Guid id)
        {
            try
            {
                var fetchedEntity = await repository.GetByIdAsync(id);
                if (fetchedEntity is null)
                {
                    return ServiceResult<ProductCustomerDto>.Failure(statusCode: 404);
                }

                return ServiceResult<ProductCustomerDto>.Success(data: fetchedEntity.ToCustomerDto());
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "[ProductService][CustomerGetSingleAsync] Unhandled Exception has occured");
                return ServiceResult<ProductCustomerDto>.Failure(
                        errors: ["Failed to fetch product from database"],
                        statusCode: 500);
            }
        }

        /// <inheritdoc/>
        public async Task<ServiceResult<List<ProductAdminDto>>> AdminsGetAllAsync()
        {
            try
            {
                var entitites = await repository.GetAllAsync();

                return ServiceResult<List<ProductAdminDto>>.Success(
                    data: entitites.Select(x => x.ToAdminDto()).ToList());
                
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "[ProductService][AdminsGetAllAsync] Unhandled Exception has occured");
                return ServiceResult<List<ProductAdminDto>>.Failure(
                        errors: ["Failed to fetch products from database"],
                        statusCode: 500);
            }
        }

        /// <inheritdoc/>
        public async Task<ServiceResult<ProductAdminDto>> AdminsGetSingleAsync(Guid id)
        {
            try
            {
                var entity = await repository.GetByIdAsync(id);
                if (entity is null)
                {
                    return ServiceResult<ProductAdminDto>.Failure(statusCode: 404);
                }

                return ServiceResult<ProductAdminDto>.Success(data: entity.ToAdminDto());
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "[ProductService][AdminsGetSingleAsync] Unhandled Exception has occured");
                return ServiceResult<ProductAdminDto>.Failure(
                        errors: ["Failed to fetch product from database"],
                        statusCode: 500);
            }
        }

        /// <inheritdoc/>
        public async Task<ServiceResult<ProductAdminDto>> AddAsync(ProductAddDto dto)
        {
            try
            {
                var entity = await repository.AddAsync(new Product
                {
                    Name = dto.Name,
                    Description = dto.Description,
                    Price = dto.Price,
                    StockQuantity = dto.StockQuantity ?? 0
                });

                await auditService.AddNewLogAsync(
                    new AuditLogAddDto
                    {
                        UserId = currentUserService.GetCurentUserId()!.Value, // user is already authorized -> its existing
                        Action = $"Added new product {entity.Id}"
                    });

                return ServiceResult<ProductAdminDto>.Success(
                    data: entity.ToAdminDto(),
                    statusCode: 201);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "[ProductService][AddAsync] Unhandled Exception has occured");
                return ServiceResult<ProductAdminDto>.Failure(
                        errors: ["Failed to create new product"],
                        statusCode: 500);
            }
        }

        /// <inheritdoc/>
        public async Task<ServiceResult<ProductAdminDto>> UpdateNameAsync(Guid id, string newName)
        {
            try
            {
                var entity = await repository.GetByIdAsync(id);
                if (entity is null)
                {
                    logger.LogWarning("[ProductService][UpdateNameAsync] Attempted name change for non-existing product");
                    return ServiceResult<ProductAdminDto>.Failure(statusCode: 404);
                }

                var oldName = entity.Name;
                entity.Name = newName;

                await repository.SaveChangesAsync();

                await auditService.AddNewLogAsync(
                    new AuditLogAddDto
                    {
                        UserId = currentUserService.GetCurentUserId()!.Value, // user is already authorized -> its existing
                        Action = $"Renamed product {entity.Id}; Old name: {oldName} - New name: {entity.Name}"
                    });

                return ServiceResult<ProductAdminDto>.Success(data: entity.ToAdminDto());
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "[ProductService][UpdateNameAsync] Unhandled Exception has occured");
                return ServiceResult<ProductAdminDto>.Failure(
                        errors: ["Failed to update product name"],
                        statusCode: 500);
            }
        }

        /// <inheritdoc/>
        public async Task<ServiceResult<ProductAdminDto>> UpdateDescriptionAsync(Guid id, string newDescription)
        {
            try
            {
                var entity = await repository.GetByIdAsync(id);
                if (entity is null)
                {
                    logger.LogWarning("[ProductService][UpdateDescriptionAsync] Attempted description change for non-existing product");
                    return ServiceResult<ProductAdminDto>.Failure(statusCode: 404);
                }

                var oldDescription = entity.Description;
                entity.Description = newDescription;

                await repository.SaveChangesAsync();

                await auditService.AddNewLogAsync(
                    new AuditLogAddDto
                    {
                        UserId = currentUserService.GetCurentUserId()!.Value, // user is already authorized -> its existing
                        Action = $"Changed product description, {entity.Id}; Old description: {oldDescription} - New description: {entity.Description}"
                    });

                return ServiceResult<ProductAdminDto>.Success(data: entity.ToAdminDto());
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "[ProductService][UpdateDescriptionAsync] Unhandled Exception has occured");
                return ServiceResult<ProductAdminDto>.Failure(
                        errors: ["Failed to update product description"],
                        statusCode: 500);
            }
        }

        /// <inheritdoc/>
        public async Task<ServiceResult<ProductAdminDto>> UpdatePriceAsync(Guid id, decimal newPrice)
        {
            try
            {
                var entity = await repository.GetByIdAsync(id);
                if (entity is null)
                {
                    logger.LogWarning("[ProductService][UpdatePriceAsync] Attempted price change for non-existing product");
                    return ServiceResult<ProductAdminDto>.Failure(statusCode: 404);
                }

                var oldPrice = entity.Price;
                entity.Price = newPrice;

                await repository.SaveChangesAsync();

                await auditService.AddNewLogAsync(
                    new AuditLogAddDto
                    {
                        UserId = currentUserService.GetCurentUserId()!.Value, // user is already authorized -> its existing
                        Action = $"Changed product price, {entity.Id}; Old price: {oldPrice} - New price: {entity.Price}"
                    });

                return ServiceResult<ProductAdminDto>.Success(data: entity.ToAdminDto());
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "[ProductService][UpdatePriceAsync] Unhandled Exception has occured");
                return ServiceResult<ProductAdminDto>.Failure(
                        errors: ["Failed to update product price"],
                        statusCode: 500);
            }
        }

        /// <inheritdoc/>
        public async Task<ServiceResult<ProductAdminDto>> UpdateStockQuantityAsync(Guid id, int newStockQuantity)
        {
            try
            {
                var entity = await repository.GetByIdAsync(id);
                if (entity is null)
                {
                    logger.LogWarning("[ProductService][UpdateStockQuantityAsync] Attempted stock change for non-existing product");
                    return ServiceResult<ProductAdminDto>.Failure(statusCode: 404);
                }

                var oldStockQuantity = entity.StockQuantity;
                entity.StockQuantity = newStockQuantity;

                await repository.SaveChangesAsync();

                await auditService.AddNewLogAsync(
                    new AuditLogAddDto
                    {
                        UserId = currentUserService.GetCurentUserId()!.Value, // user is already authorized -> its existing
                        Action = $"Changed product stock quantity, {entity.Id}; Old stock quantity: {oldStockQuantity} - New stock quantity: {entity.StockQuantity}"
                    });

                return ServiceResult<ProductAdminDto>.Success(data: entity.ToAdminDto());
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "[ProductService][UpdateStockQuantityAsync] Unhandled Exception has occured");

                return ServiceResult<ProductAdminDto>.Failure(
                        errors: ["Failed to update product stock quantity"],
                        statusCode: 500);
            }
        }

        /// <inheritdoc/>
        public async Task<ServiceResult<ProductAdminDto>> UpdateAsync(Guid id, ProductUpdateDto dto)
        {
            try
            {
                var entity = await repository.GetByIdAsync(id);

                if (entity is null)
                {
                    logger.LogWarning("[ProductService][UpdateAsync] Attempted name change for non-existing product");
                    return ServiceResult<ProductAdminDto>.Failure(statusCode: 404);
                }

                var newValues = new List<string>();

                if (dto.Name is not null)
                {
                    entity.Name = dto.Name;
                    newValues.Add(dto.Name);
                };
                if (dto.Description is not null)
                {
                    entity.Description = dto.Description;
                    newValues.Add(dto.Description);
                };
                if (dto.Price is not null)
                {
                    entity.Price = dto.Price.Value;
                    newValues.Add(dto.Price.Value.ToString());
                };
                if (dto.StockQuantity is not null)
                {
                    entity.StockQuantity = dto.StockQuantity.Value;
                    newValues.Add(dto.StockQuantity.Value.ToString());
                };

                await repository.SaveChangesAsync();

                await auditService.AddNewLogAsync(
                    new AuditLogAddDto
                    {
                        UserId = currentUserService.GetCurentUserId()!.Value, // user is already authorized -> its existing
                        Action = $"Changed product information {entity.Id}, new values {string.Join(";", newValues)};"
                    });

                return ServiceResult<ProductAdminDto>.Success(data: entity.ToAdminDto());
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "[ProductService][UpdateAsync] Unhandled Exception has occured");
                return ServiceResult<ProductAdminDto>.Failure(
                        errors: ["Failed to update product"],
                        statusCode: 500);
            }
        }

        /// <inheritdoc/>
        public async Task<ServiceResult<ProductAdminDto>> DeleteAsync(Guid id)
        {
            try
            {
                var entity = await repository.GetByIdAsync(id);

                if (entity is null)
                {
                    logger.LogWarning("[ProductService][DeleteAsync] Attempted deletion of non-existing product");
                    return ServiceResult<ProductAdminDto>.Failure(statusCode: 404);
                }

                var entityId = entity.Id;

                await repository.DeleteAsync(entity);

                await auditService.AddNewLogAsync(
                    new AuditLogAddDto
                    {
                        UserId = currentUserService.GetCurentUserId()!.Value, // user is already authorized -> its existing
                        Action = $"Added new product {entityId}"
                    });

                return ServiceResult<ProductAdminDto>.Success(statusCode: 204);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "[ProductService][DeleteAsync] Unhandled Exception has occured");
                return ServiceResult<ProductAdminDto>.Failure(
                        errors: ["Failed to delete product"],
                        statusCode: 500);
            }
        }
    }
}