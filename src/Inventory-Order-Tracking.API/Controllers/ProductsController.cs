using Azure.Core;
using FluentValidation;
using Inventory_Order_Tracking.API.Domain;
using Inventory_Order_Tracking.API.Dtos;
using Inventory_Order_Tracking.API.Services.Interfaces;
using Inventory_Order_Tracking.API.Validators;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Inventory_Order_Tracking.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class ProductsController(
        IProductService service,
        StringValueValidator stringValueValidator,
        ILogger<ProductsController> logger) : ControllerBase
    {

        [HttpGet("customer")]
        [Authorize(Policy = "customer")]
        public async Task<IActionResult> CustomerGetAll()
        {
            var serviceResult = await service.CustomersGetAllAsync();

            if(!serviceResult.IsSuccessful)
                return StatusCode((int)serviceResult.StatusCode, serviceResult.ErrorMessage);

            return Ok(serviceResult.Data);

        }
        [HttpGet("customer/{id:guid}")]
        [Authorize(Policy = "customer")]
        public async Task<IActionResult> CustomerGetSingle(Guid id)
        {
            var serviceResult = await service.CustomersGetSingleAsync(id);

            if (!serviceResult.IsSuccessful)
                return StatusCode((int)serviceResult.StatusCode, serviceResult.ErrorMessage);

            return Ok(serviceResult.Data);
        }
        [HttpGet("admin")]
        [Authorize(Policy = "admin")]
        public async Task<IActionResult> AdminsGetAll()
        {
            var serviceResult = await service.AdminsGetAllAsync();

            if (!serviceResult.IsSuccessful)
                return StatusCode((int)serviceResult.StatusCode, serviceResult.ErrorMessage);

            return Ok(serviceResult.Data);
        }
        [HttpGet("admin/{id:guid}")]
        [Authorize(Policy = "admin")]
        public async Task<IActionResult> AdminsGetSingle(Guid id)
        {
            var serviceResult = await service.AdminsGetSingleAsync(id);

            if (!serviceResult.IsSuccessful)
                return StatusCode((int)serviceResult.StatusCode, serviceResult.ErrorMessage);

            return Ok(serviceResult.Data);
        }

        [HttpPatch("admin/update-name/{id:guid}")]
        [Authorize(Roles = UserRoles.Admin)]
        public async Task<IActionResult> AdminsUpdateName([FromQuery] Guid id, ProductUpdateNameDto dto)
        {
            var validationResult = stringValueValidator.Validate(new StringWrapper { Value=dto.Name});

            if (!validationResult.IsValid)
            {
                logger.LogWarning("[ProductsController][AdminsUpdateName] Invalid rename attempt for {id}. Encountered errors: {errors}",
                    id,
                    string.Join("; ", validationResult.Errors.Select(e => e.ErrorMessage)));

                return BadRequest(new { errors = validationResult.Errors.Select(e => e.ErrorMessage) });
            }

            var serviceResult = await service.UpdateNameAsync(id, dto.Name);

            return serviceResult.IsSuccessful
                ? Ok(serviceResult.Data)
                : StatusCode((int)serviceResult.StatusCode, serviceResult.ErrorMessage);


        }
        [HttpPatch("admin/update-description/{id:guid}")]
        [Authorize(Roles = UserRoles.Admin)]
        public async Task<IActionResult> AdminsUpdateDescription([FromQuery] Guid id, ProductUpdateDescription dto)
        {
            var validationResult = stringValueValidator.Validate(new StringWrapper { Value = dto.Description });

            if (!validationResult.IsValid)
            {
                logger.LogWarning("[ProductsController][AdminsUpdateDescription] Invalid description change attempt for {id}. Encountered errors: {errors}",
                    id,
                    string.Join("; ", validationResult.Errors.Select(e => e.ErrorMessage)));

                return BadRequest(new { errors = validationResult.Errors.Select(e => e.ErrorMessage) });
            }

            var serviceResult = await service.UpdateDescriptionAsync(id, dto.Description);

            return serviceResult.IsSuccessful
                ? Ok(serviceResult.Data)
                : StatusCode((int)serviceResult.StatusCode, serviceResult.ErrorMessage);
        }
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
        [HttpPatch("admin/update-stock/{id:guid}")]
        [Authorize(Roles = UserRoles.Admin)]
        public async Task<IActionResult> UpdateStock([FromQuery] Guid id, ProductUpdateStockDto dto)
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
        [HttpPut("admin/delete/{id:guid}")]
        [Authorize(Roles = UserRoles.Admin)]
        public async Task<IActionResult> AdminsDelete([FromQuery] Guid id)
        {
            var serviceResult = await service.DeleteAsync(id);

            return serviceResult.IsSuccessful
                ? Ok(serviceResult.Data)
                : StatusCode((int)serviceResult.StatusCode, serviceResult.ErrorMessage);
        }


    }
}
