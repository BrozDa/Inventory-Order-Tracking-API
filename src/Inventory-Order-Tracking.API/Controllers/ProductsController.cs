using Inventory_Order_Tracking.API.Domain;
using Inventory_Order_Tracking.API.Dtos;
using Inventory_Order_Tracking.API.Services.Interfaces;
using Inventory_Order_Tracking.API.Services.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Inventory_Order_Tracking.API.Controllers
{
    /// <summary>
    /// Controller responsible for handling operations for products.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class ProductsController(
        IProductService service,
        ILogger<ProductsController> logger) : ControllerBase
    {

        /// <summary>
        /// Retrieves all products in customerDto format.
        /// </summary>
        /// <returns>A service result containing list of all products in data field.</returns>
        /// <response code="200">List of all products.</response>
        /// <response code="401">Not authenticated.</response>
        /// <response code="500">An unexpected server-side error occurred.</response>
        [HttpGet("customer")]
        [Authorize(Policy = "customer")]
        public async Task<IActionResult> CustomerGetAll()
        {
            var serviceResult = await service.CustomersGetAllAsync();

            return StatusCode(serviceResult.StatusCode, serviceResult);
        }

        /// <summary>
        /// Retrieves single product in customerDto format.
        /// </summary>
        /// <param name="id">Id of product to be retrieved</param>
        /// <returns>A service result containing the retrieved product in data field.</returns>
        /// <response code="200">A retrieved product.</response>
        /// <response code="401">Not authenticated.</response>
        /// <response code="404">Non existent order.</response>
        /// <response code="500">An unexpected server-side error occurred.</response>
        [HttpGet("customer/{id:guid}")]
        [Authorize(Policy = "customer")]
        public async Task<IActionResult> CustomerGetSingle(Guid id)
        {
            var serviceResult = await service.CustomersGetSingleAsync(id);

            return StatusCode(serviceResult.StatusCode, serviceResult);
        }

        /// <summary>
        /// Retrieves all products in adminDto format.
        /// </summary>
        /// <returns>A service result containing list of all products in data field.</returns>
        /// <response code="200">List of all products.</response>
        /// <response code="401">Not authenticated.</response>
        /// <response code="403">Not admin role.</response>
        /// <response code="500">An unexpected server-side error occurred.</response>
        [HttpGet("admin")]
        [Authorize(Policy = "admin")]
        public async Task<IActionResult> AdminsGetAll()
        {
            var serviceResult = await service.AdminsGetAllAsync();

            return StatusCode(serviceResult.StatusCode, serviceResult);
        }
        /// <summary>
        /// Retrieves single product in adminDto format.
        /// </summary>
        /// <returns>A service result containing retrieved product in data field.</returns>
        /// <response code="200">List of all products.</response>
        /// <response code="401">Not authenticated.</response>
        /// <response code="403">Not admin role.</response>
        /// <response code="404">Non existent order.</response>
        /// <response code="500">An unexpected server-side error occurred.</response>
        [HttpGet("admin/{id:guid}")]
        [Authorize(Policy = "admin")]
        public async Task<IActionResult> AdminsGetSingle(Guid id)
        {
            var serviceResult = await service.AdminsGetSingleAsync(id);

            return StatusCode(serviceResult.StatusCode, serviceResult);
        }
        /// <summary>
        /// Updates product name.
        /// </summary>
        /// <param name="id">Id of product to be updated</param>
        /// <param name="dto">An ProductUpdateNameDto containing new product name</param>
        /// <returns>A service result containing updated product in data field.</returns>
        /// <response code="200">An updated product.</response>
        /// <response code="400">Product name validation failed.</response>
        /// <response code="401">Not authenticated.</response>
        /// <response code="403">Not admin role.</response>
        /// <response code="404">Non existent product.</response>
        /// <response code="500">An unexpected server-side error occurred.</response>
        [HttpPatch("admin/update-name/{id:guid}")]
        [Authorize(Roles = UserRoles.Admin)]
        public async Task<IActionResult> AdminsUpdateName(Guid id, ProductUpdateNameDto dto)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToList();

                logger.LogWarning("[ProductsController][AdminsUpdateName] Validation failed: {@errors}", errors);
                return StatusCode(400, ServiceResult<ProductAdminDto?>.Failure(
                    errors: errors));
            }

            var serviceResult = await service.UpdateNameAsync(id, dto.Name);

            return StatusCode(serviceResult.StatusCode, serviceResult);
        }
        /// <summary>
        /// Updates product description.
        /// </summary>
        /// <param name="id">Id of product to be updated</param>
        /// <param name="dto">An ProductUpdateDescriptionDto containing new product description</param>
        /// <returns>A service result containing updated product in data field.</returns>
        /// <response code="200">An updated product.</response>
        /// <response code="400">Product description validation failed.</response>
        /// <response code="401">Not authenticated.</response>
        /// <response code="403">Not admin role.</response>
        /// <response code="404">Non existent product.</response>
        /// <response code="500">An unexpected server-side error occurred.</response>

        [HttpPatch("admin/update-description/{id:guid}")]
        [Authorize(Roles = UserRoles.Admin)]
        public async Task<IActionResult> AdminsUpdateDescription(Guid id, ProductUpdateDescriptionDto dto)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToList();

                logger.LogWarning("[ProductsController][AdminsUpdateDescription] Validation failed: {@errors}", errors);
                return StatusCode(400, ServiceResult<ProductAdminDto?>.Failure(
                    errors: errors));
            }

            var serviceResult = await service.UpdateDescriptionAsync(id, dto.Description);

            return StatusCode(serviceResult.StatusCode, serviceResult);
        }
        /// <summary>
        /// Updates product price.
        /// </summary>
        /// <param name="id">Id of product to be updated</param>
        /// <param name="dto">An ProductUpdatePriceDto containing new product price</param>
        /// <returns>A service result containing updated product in data field.</returns>
        /// <response code="200">An updated product.</response>
        /// <response code="400">Product price validation failed.</response>
        /// <response code="401">Not authenticated.</response>
        /// <response code="403">Not admin role.</response>
        /// <response code="404">Non existent product.</response>
        /// <response code="500">An unexpected server-side error occurred.</response>
        [HttpPatch("admin/update-price/{id:guid}")]
        [Authorize(Roles = UserRoles.Admin)]
        public async Task<IActionResult> AdminsUpdatePrice(Guid id, ProductUpdatePriceDto dto)
        {
            if (dto.Price < 0)
            {
                logger.LogWarning("[ProductsController][AdminsUpdatePrice] Invalid price change attempt for {id}. Attempted price {price}",
                    id,
                    dto.Price);

                return StatusCode(400, ServiceResult<ProductAdminDto?>.Failure(
                    errors: ["Price must not be negative number"]));
  
            }

            var serviceResult = await service.UpdatePriceAsync(id, dto.Price);

            return StatusCode(serviceResult.StatusCode, serviceResult);
        }
        /// <summary>
        /// Updates product stock quantity.
        /// </summary>
        /// <param name="id">Id of product to be updated</param>
        /// <param name="dto">An ProductUpdateStockDto containing new product stock quantity</param>
        /// <returns>A service result containing updated product in data field.</returns>
        /// <response code="200">An updated product.</response>
        /// <response code="400">Product stock quantity validation failed.</response>
        /// <response code="401">Not authenticated.</response>
        /// <response code="403">Not admin role.</response>
        /// <response code="404">Non existent product.</response>
        /// <response code="500">An unexpected server-side error occurred.</response>
        [HttpPatch("admin/update-stock/{id:guid}")]
        [Authorize(Roles = UserRoles.Admin)]
        public async Task<IActionResult> AdminsUpdateStock(Guid id, ProductUpdateStockDto dto)
        {
            if (dto.Stock < 0)
            {
                logger.LogWarning("[ProductsController][AdminsUpdatePrice] Invalid stock change attempt for {id}. Attempted stock {stock}",
                    id,
                    dto.Stock);

                return StatusCode(400, ServiceResult<ProductAdminDto?>.Failure(
                    errors: ["Stock must not be negative number"]));
            }

            var serviceResult = await service.UpdateStockQuantityAsync(id, dto.Stock);

            return StatusCode(serviceResult.StatusCode, serviceResult);
        }

        /// <summary>
        /// Updates multiple values of a product.
        /// </summary>
        /// <param name="id">Id of product to be updated</param>
        /// <param name="dto">An ProductUpdateDto containing values to be updated</param>
        /// <returns>A service result containing updated product in data field.</returns>
        /// <response code="200">An updated product.</response>
        /// <response code="400">Product updated properties validation failed.</response>
        /// <response code="401">Not authenticated.</response>
        /// <response code="403">Not admin role.</response>
        /// <response code="404">Non existent product.</response>
        /// <response code="500">An unexpected server-side error occurred.</response>
        [HttpPut("admin/update/{id:guid}")]
        [Authorize(Roles = UserRoles.Admin)]
        public async Task<IActionResult> AdminsUpdate(Guid id, ProductUpdateDto dto)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToList();

                logger.LogWarning("[ProductsController][AdminsUpdate] Validation failed: {@errors}", errors);
                return StatusCode(400, ServiceResult<ProductAdminDto?>.Failure(
                    errors: errors));
            }

            var serviceResult = await service.UpdateAsync(id, dto);

            return StatusCode(serviceResult.StatusCode, serviceResult);
        }
        /// <summary>
        /// Adds a new product.
        /// </summary>
        /// <param name="dto">An ProductAddDto containing new product information</param>
        /// <returns>A service result containing newly added product in data field.</returns>
        /// <response code="201">A newly added product.</response>
        /// <response code="400">New product properties validation failed.</response>
        /// <response code="401">Not authenticated.</response>
        /// <response code="403">Not admin role.</response>
        /// <response code="500">An unexpected server-side error occurred.</response>
        [HttpPost("admin/add")]
        [Authorize(Roles = UserRoles.Admin)]
        public async Task<IActionResult> AdminsAdd(ProductAddDto dto)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToList();

                logger.LogWarning("[ProductsController][AdminsAddStock] Validation failed: {@errors}", errors);

                return StatusCode(400, ServiceResult<ProductAdminDto?>.Failure(
                    errors: errors));
            }

            var serviceResult = await service.AddAsync(dto);

            return StatusCode(serviceResult.StatusCode, serviceResult);
        }
        /// <summary>
        /// Deletes existing product.
        /// </summary>
        /// <param name="id">An id of the product to be deleted</param>
        /// <returns>A service result containing deleted product in data field.</returns>
        /// <response code="204"></response>
        /// <response code="401">Not authenticated.</response>
        /// <response code="403">Not admin role.</response>
        /// <response code="404">Non existent product.</response>
        /// <response code="500">An unexpected server-side error occurred.</response>
        [HttpDelete("admin/delete/{id:guid}")]
        [Authorize(Roles = UserRoles.Admin)]
        public async Task<IActionResult> AdminsDelete(Guid id)
        {
            var serviceResult = await service.DeleteAsync(id);

            return StatusCode(serviceResult.StatusCode, serviceResult);
        }
    }
}