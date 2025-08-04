using Inventory_Order_Tracking.API.Dtos;
using Inventory_Order_Tracking.API.Models;
using Inventory_Order_Tracking.API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Inventory_Order_Tracking.API.Controllers
{
    public class OrderController(
        ICurrentUserService userService,
        IOrderService orderService) :ControllerBase
    {
        [HttpPost]
        [Authorize]

        public async Task<IActionResult> PlaceOrder([FromBody] CreateOrderDto orderDto)
        {
            var userId = userService.GetCurentUserId();
            if (userId is null)
                return Unauthorized("User Id not found in the token");

            var serviceResult = await orderService.SubmitOrder(userId.Value, orderDto);

            return serviceResult.IsSuccessful
            ? CreatedAtAction(nameof(GetOrderById), new { id = serviceResult.Data!.Id }, serviceResult.Data)
                : StatusCode((int)serviceResult.StatusCode, serviceResult.ErrorMessage);
        }
        [HttpGet]
        [Authorize]
        [HttpGet("{orderId:guid}")]
        public async Task<IActionResult> GetOrderById([FromQuery] Guid id) 
        {
            throw new NotImplementedException();
        }
    }
}
