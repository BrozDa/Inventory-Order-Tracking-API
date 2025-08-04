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

        [HttpGet("{orderId:guid}")]
        [Authorize]
        public async Task<IActionResult> GetOrderById(Guid orderId)
        {
            var userId = userService.GetCurentUserId();
            if (userId is null)
                return Unauthorized("User Id not found in the token");

            var serviceResult = await orderService.GetOrderById(userId.Value, orderId);

            return serviceResult.IsSuccessful
            ? Ok(serviceResult.Data)
                : StatusCode((int)serviceResult.StatusCode, serviceResult.ErrorMessage);
        }

        [HttpGet("all")]
        [Authorize] // Users can view their own order history
        public async Task<IActionResult> GetOrderHistoryForUser()
        {
            var userId = userService.GetCurentUserId();
            if (userId is null)
                return Unauthorized("User Id not found in the token");

            var serviceResult = await orderService.GetAllOrdersForUser(userId.Value);

            return serviceResult.IsSuccessful
            ? Ok(serviceResult.Data)
                : StatusCode((int)serviceResult.StatusCode, serviceResult.ErrorMessage);
        }

    }
}
