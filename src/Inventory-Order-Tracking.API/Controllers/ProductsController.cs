using Inventory_Order_Tracking.API.Domain;
using Inventory_Order_Tracking.API.Dtos;
using Inventory_Order_Tracking.API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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

            if (!serviceResult.IsSuccessful)
                return StatusCode((int)serviceResult.StatusCode, serviceResult.ErrorMessage);

            return Ok(serviceResult.Data);
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

            if (!serviceResult.IsSuccessful)
                return StatusCode((int)serviceResult.StatusCode, serviceResult.ErrorMessage);

            return Ok(serviceResult.Data);
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

            if (!serviceResult.IsSuccessful)
                return StatusCode((int)serviceResult.StatusCode, serviceResult.ErrorMessage);

            return Ok(serviceResult.Data);
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

            if (!serviceResult.IsSuccessful)
                return StatusCode((int)serviceResult.StatusCode, serviceResult.ErrorMessage);

            return Ok(serviceResult.Data);
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
                return BadRequest(new { Errors = errors });
            }

            var serviceResult = await service.UpdateNameAsync(id, dto.Name);

            return serviceResult.IsSuccessful
                ? Ok(serviceResult.Data)
                : StatusCode((int)serviceResult.StatusCode, serviceResult.ErrorMessage);
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
                return BadRequest(new { Errors = errors });
            }

            var serviceResult = await service.UpdateDescriptionAsync(id, dto.Description);

            return serviceResult.IsSuccessful
                ? Ok(serviceResult.Data)
                : StatusCode((int)serviceResult.StatusCode, serviceResult.ErrorMessage);
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

                return BadRequest("Price must not be negative number");
            }

            var serviceResult = await service.UpdatePriceAsync(id, dto.Price);

            return serviceResult.IsSuccessful
                ? Ok(serviceResult.Data)
                : StatusCode((int)serviceResult.StatusCode, serviceResult.ErrorMessage);
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

                return BadRequest("Stock must not be negative number");
            }

            var serviceResult = await service.UpdateStockQuantityAsync(id, dto.Stock);

            return serviceResult.IsSuccessful
                ? Ok(serviceResult.Data)
                : StatusCode((int)serviceResult.StatusCode, serviceResult.ErrorMessage);
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
                return BadRequest(new { Errors = errors });
            }

            var serviceResult = await service.UpdateAsync(id, dto);

            return serviceResult.IsSuccessful
                ? Ok(serviceResult.Data)
                : StatusCode((int)serviceResult.StatusCode, serviceResult.ErrorMessage);
        }

        /// <summary>
        /// Adds a new product.
        /// </summary>
        /// <param name="dto">An <see cref="ProductAddDto"/> containing values for product to be added</param>
        /// <returns>
        /// An Created <see cref="IActionResult"/> on success.
        /// Returns an appropriate status code and error message on failure.
        /// </returns>
        [HttpPut("admin/add")]
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
                return BadRequest(new { Errors = errors });
            }

            var serviceResult = await service.AddAsync(dto);

            return serviceResult.IsSuccessful
                ? Created()
                : StatusCode((int)serviceResult.StatusCode, serviceResult.ErrorMessage);
        }

        /// <summary>
        /// Deletes existing product.
        /// </summary>
        /// <param name="id">An <see cref="Guid"/> of the product to be deleted</param>
        /// <returns>
        /// An NoContent <see cref="IActionResult"/> on success.
        /// Returns an appropriate status code and error message on failure.
        /// </returns>
        [HttpPut("admin/delete/{id:guid}")]
        [Authorize(Roles = UserRoles.Admin)]
        public async Task<IActionResult> AdminsDelete([FromQuery] Guid id)
        {
            var serviceResult = await service.DeleteAsync(id);

            return serviceResult.IsSuccessful
                ? NoContent()
                : StatusCode((int)serviceResult.StatusCode, serviceResult.ErrorMessage);
        }
    }
}