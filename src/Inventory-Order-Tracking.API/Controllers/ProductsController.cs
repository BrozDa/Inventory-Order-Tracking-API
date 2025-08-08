using Inventory_Order_Tracking.API.Domain;
using Inventory_Order_Tracking.API.Dtos;
using Inventory_Order_Tracking.API.Services.Interfaces;
using Inventory_Order_Tracking.API.Services.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Inventory_Order_Tracking.API.Controllers
{
    /// <summary>
    /// Controller responsible for handling operations for products.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController(
        IProductService service,
        ILogger<ProductsController> logger) : ControllerBase
    {
        /// <summary>
        /// Retrieves all products in form of <see cref="ProductCustomerDto"/>.
        /// </summary>
        /// <returns>
        /// An Ok <see cref="IActionResult"/> containing a list of all products on success.
        /// Returns an appropriate status code and error message on failure.
        /// </returns>
        [HttpGet("customer")]
        [Authorize(Policy = "customer")]
        public async Task<IActionResult> CustomerGetAll()
        {
            var serviceResult = await service.CustomersGetAllAsync();

            return StatusCode(serviceResult.StatusCode, serviceResult);
        }

        /// <summary>
        /// Retrieves single product based on provided id in form of <see cref="ProductCustomerDto"/>.
        /// </summary>
        /// <param name="id">Id of product to be retrieved</param>
        /// <returns>
        /// An Ok <see cref="IActionResult"/> containing retrieved product on success.
        /// Returns an appropriate status code and error message on failure.
        /// </returns>
        [HttpGet("customer/{id:guid}")]
        [Authorize(Policy = "customer")]
        public async Task<IActionResult> CustomerGetSingle(Guid id)
        {
            var serviceResult = await service.CustomersGetSingleAsync(id);

            return StatusCode(serviceResult.StatusCode, serviceResult);
        }

        /// <summary>
        /// Retrieves all products in form of <see cref="ProductAdminDto"/>.
        /// </summary>
        /// <returns>
        /// An Ok <see cref="IActionResult"/> containing a list of all products on success.
        /// Returns an appropriate status code and error message on failure.
        /// </returns>
        [HttpGet("admin")]
        [Authorize(Policy = "admin")]
        public async Task<IActionResult> AdminsGetAll()
        {
            var serviceResult = await service.AdminsGetAllAsync();

            return StatusCode(serviceResult.StatusCode, serviceResult);
        }

        /// <summary>
        /// Retrieves single product based on provided id in form of <see cref="ProductAdminDto"/>.
        /// </summary>
        /// <param name="id">Id of product to be retrieved</param>
        /// <returns>
        /// An Ok <see cref="IActionResult"/> containing retrieved product on success.
        /// Returns an appropriate status code and error message on failure.
        /// </returns>
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
        /// <param name="dto">An <see cref="ProductUpdateNameDto"/> containing new product name</param>
        /// <returns>
        /// An Ok <see cref="IActionResult"/> containing updated product on success.
        /// Returns an appropriate status code and error message on failure.
        /// </returns>
        [HttpPatch("admin/update-name/{id:guid}")]
        [Authorize(Roles = UserRoles.Admin)]
        public async Task<IActionResult> AdminsUpdateName([FromQuery] Guid id, ProductUpdateNameDto dto)
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
        /// <param name="dto">An <see cref="ProductUpdateDescriptionDto"/> containing new product description</param>
        /// <returns>
        /// An Ok <see cref="IActionResult"/> containing updated product on success.
        /// Returns an appropriate status code and error message on failure.
        /// </returns>
        [HttpPatch("admin/update-description/{id:guid}")]
        [Authorize(Roles = UserRoles.Admin)]
        public async Task<IActionResult> AdminsUpdateDescription([FromQuery] Guid id, ProductUpdateDescriptionDto dto)
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
        /// <param name="dto">An <see cref="ProductUpdatePriceDto"/> containing new product price</param>
        /// <returns>
        /// An Ok <see cref="IActionResult"/> containing updated product on success.
        /// Returns an appropriate status code and error message on failure.
        /// </returns>
        [HttpPatch("admin/update-price/{id:guid}")]
        [Authorize(Roles = UserRoles.Admin)]
        public async Task<IActionResult> AdminsUpdatePrice([FromQuery] Guid id, ProductUpdatePriceDto dto)
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
        /// <param name="dto">An <see cref="ProductUpdateStockDto"/> containing new product stock quantity</param>
        /// <returns>
        /// An Ok <see cref="IActionResult"/> containing updated product on success.
        /// Returns an appropriate status code and error message on failure.
        /// </returns>
        [HttpPatch("admin/update-stock/{id:guid}")]
        [Authorize(Roles = UserRoles.Admin)]
        public async Task<IActionResult> AdminsUpdateStock([FromQuery] Guid id, ProductUpdateStockDto dto)
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
        /// <param name="dto">An <see cref="ProductUpdateDto"/> containing values to be updated</param>
        /// <returns>
        /// An Ok <see cref="IActionResult"/> containing updated product on success.
        /// Returns an appropriate status code and error message on failure.
        /// </returns>
        [HttpPut("admin/update/{id:guid}")]
        [Authorize(Roles = UserRoles.Admin)]
        public async Task<IActionResult> AdminsUpdate([FromQuery] Guid id, ProductUpdateDto dto)
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
        /// <param name="dto">An <see cref="ProductAddDto"/> containing values for product to be added</param>
        /// <returns>
        /// An Created <see cref="IActionResult"/> on success.
        /// Returns an appropriate status code and error message on failure.
        /// </returns>
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
        /// <param name="id">An <see cref="Guid"/> of the product to be deleted</param>
        /// <returns>
        /// An NoContent <see cref="IActionResult"/> on success.
        /// Returns an appropriate status code and error message on failure.
        /// </returns>
        [HttpDelete("admin/delete/{id:guid}")]
        [Authorize(Roles = UserRoles.Admin)]
        public async Task<IActionResult> AdminsDelete([FromQuery] Guid id)
        {
            var serviceResult = await service.DeleteAsync(id);

            return StatusCode(serviceResult.StatusCode, serviceResult);
        }
    }
}