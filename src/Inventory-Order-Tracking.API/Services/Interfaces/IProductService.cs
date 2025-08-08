using Inventory_Order_Tracking.API.Dtos;
using Inventory_Order_Tracking.API.Services.Shared;

namespace Inventory_Order_Tracking.API.Services.Interfaces
{
    /// <summary>
    /// Defines a contract for operations for retrieving and managing products within the app.
    /// </summary>
    public interface IProductService
    {
        /// <summary>
        /// Retrieves all products for admin user
        /// </summary>
        /// <returns>
        /// An <see cref="ServiceResult{T}"/> containing list of existing <see cref="ProductAdminDto"/> on success,
        /// or an appropriate error message on failure.
        /// </returns>
        Task<ServiceResult<List<ProductAdminDto>>> AdminsGetAllAsync();

        /// <summary>
        /// Retrieves single product for admin user
        /// </summary>
        /// <param name="id">An id of product to be retrieved</param>
        /// <returns>
        /// An <see cref="ServiceResult{T}"/> containing an existing <see cref="ProductAdminDto"/> on success,
        /// or an appropriate error message on failure.
        /// </returns>
        Task<ServiceResult<ProductAdminDto>> AdminsGetSingleAsync(Guid id);

        /// <summary>
        /// Retrieves all products for customer user
        /// </summary>
        /// <returns>
        /// An <see cref="ServiceResult{T}"/> containing list of existing <see cref="ProductCustomerDto"/> on success,
        /// or an appropriate error message on failure.
        /// </returns>
        Task<ServiceResult<List<ProductCustomerDto>>> CustomersGetAllAsync();

        /// <summary>
        /// Retrieves single customer for admin user
        /// </summary>
        /// <param name="id">An id of product to be retrieved</param>
        /// <returns>
        /// An <see cref="ServiceResult{T}"/> containing an existing <see cref="ProductAdminDto"/> on success,
        /// or an appropriate error message on failure.
        /// </returns>
        Task<ServiceResult<ProductCustomerDto>> CustomersGetSingleAsync(Guid id);

        /// <summary>
        /// Updates one or more values of an existing product
        /// </summary>
        /// <param name="id">An id of product to be updated</param>
        /// <param name="dto">A <see cref="ProductCustomerDto"/> containing new values of product
        /// (values which have to remain same should be set to null in the dto</param>
        /// <returns>
        /// An <see cref="ServiceResult{T}"/> containing an updated <see cref="ProductAdminDto"/> on success,
        /// or an appropriate error message on failure.
        /// </returns>
        Task<ServiceResult<ProductAdminDto>> UpdateAsync(Guid id, ProductUpdateDto dto);

        /// <summary>
        /// Updates name of existing product
        /// </summary>
        /// <param name="id">An id of product to be updated</param>
        /// <param name="newName">A <see cref="string"/> containing new name of the product
        /// <returns>
        /// An <see cref="ServiceResult{T}"/> containing an updated <see cref="ProductAdminDto"/> on success,
        /// or an appropriate error message on failure.
        /// </returns>
        Task<ServiceResult<ProductAdminDto>> UpdateNameAsync(Guid id, string newName);

        /// <summary>
        /// Updates description of existing product
        /// </summary>
        /// <param name="id">An id of product to be updated</param>
        /// <param name="newDescription">A <see cref="string"/> containing new description of the product
        /// <returns>
        /// An <see cref="ServiceResult{T}"/> containing an updated <see cref="ProductAdminDto"/> on success,
        /// or an appropriate error message on failure.
        /// </returns>
        Task<ServiceResult<ProductAdminDto>> UpdateDescriptionAsync(Guid id, string newDescription);

        /// <summary>
        /// Updates price of existing product
        /// </summary>
        /// <param name="id">An id of product to be updated</param>
        /// <param name="newPrice">A <see cref="decimal"/> containing new price of the product
        /// <returns>
        /// An <see cref="ServiceResult{T}"/> containing an updated <see cref="ProductAdminDto"/> on success,
        /// or an appropriate error message on failure.
        /// </returns>
        Task<ServiceResult<ProductAdminDto>> UpdatePriceAsync(Guid id, decimal newPrice);

        /// <summary>
        /// Updates stock of existing product
        /// </summary>
        /// <param name="id">An id of product to be updated</param>
        /// <param name="newStockQuantity">A <see cref="int"/> containing new stock of the product
        /// <returns>
        /// An <see cref="ServiceResult{T}"/> containing an updated <see cref="ProductAdminDto"/> on success,
        /// or an appropriate error message on failure.
        /// </returns>
        Task<ServiceResult<ProductAdminDto>> UpdateStockQuantityAsync(Guid id, int newStockQuantity);

        /// <summary>
        /// Adds a new product to the data storage
        /// </summary>
        /// <param name="dto">An <see cref="ProductAddDto"/> containing all necessary information about the new product</param>
        // <returns>
        /// An <see cref="ServiceResult{T}"/> containing the new <see cref="ProductAddDto"/> on success,
        /// or an appropriate error message on failure.
        /// </returns>
        Task<ServiceResult<ProductAdminDto>> AddAsync(ProductAddDto dto);

        /// <summary>
        /// Deletes existing product from the data storage
        /// </summary>
        /// <param name="id">An Id of the product to be deleted</param>
        // <returns>
        /// An <see cref="ServiceResult{T}"/> containing the deleted <see cref="ProductAddDto"/> on success,
        /// or an appropriate error message on failure.
        /// </returns>
        Task<ServiceResult<ProductAdminDto>> DeleteAsync(Guid id);
    }
}