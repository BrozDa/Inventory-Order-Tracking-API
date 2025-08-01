using Inventory_Order_Tracking.API.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Inventory_Order_Tracking.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ProductController :ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {}
        [HttpGet("{productId}")]
        public async Task<IActionResult> GetAll(Guid productId)
        { }


    }
}
