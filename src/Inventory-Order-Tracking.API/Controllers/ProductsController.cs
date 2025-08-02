using Inventory_Order_Tracking.API.Domain;
using Inventory_Order_Tracking.API.Dtos;
using Inventory_Order_Tracking.API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Inventory_Order_Tracking.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class ProductsController(IProductService service) : ControllerBase
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









        [HttpGet("admin/update-name/{id:guid}")]
        [Authorize(Roles = UserRoles.Admin)]
        public async Task<IActionResult> AdminsUpdateName(string newName)
        {
            throw new NotImplementedException();
        }
        [HttpGet("admin/update-description/{id:guid}")]
        [Authorize(Roles = UserRoles.Admin)]
        public async Task<IActionResult> AdminsUpdateDescription(string newDescription)
        {
            throw new NotImplementedException();
        }
        [HttpGet("admin/update-price/{id:guid}")]
        [Authorize(Roles = UserRoles.Admin)]
        public async Task<IActionResult> AdminsUpdatePrice(decimal newPrice)
        {
            throw new NotImplementedException();
        }
        [HttpGet("admin/update-stock/{id:guid}")]
        [Authorize(Roles = UserRoles.Admin)]
        public async Task<IActionResult> AdminsAddStock(int newStockQuantity)
        {
            throw new NotImplementedException();
        }
        [HttpGet("admin/update/{id:guid}")]
        [Authorize(Roles = UserRoles.Admin)]
        public async Task<IActionResult> AdminsAddStock(UpdateProductDto dto)
        {
            throw new NotImplementedException();
        }

    }
}
