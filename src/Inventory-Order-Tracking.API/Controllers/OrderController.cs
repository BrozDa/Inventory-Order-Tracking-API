using Inventory_Order_Tracking.API.Dtos;
using Inventory_Order_Tracking.API.Services.Interfaces;
using Inventory_Order_Tracking.API.Services.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Inventory_Order_Tracking.API.Controllers
{
    /// <summary>
    /// Controller responsible for order handling operations.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class OrderController(
        ICurrentUserService userService,
        IOrderService orderService) : ControllerBase
    {

        /// <summary>
        /// Submits new order for an user.
        /// </summary>
        /// <param name="orderDto">A list of of ordered items and quantities </param>
        /// <returns>A service result containing information about submitted order  in data field.</returns>
        /// <response code="201">Order successfully submitted.</response>
        /// <response code="400">Errors encountered during validation.</response>
        /// <response code="401">Invalid JWT token.</response>
        /// <response code="500">An unexpected server-side error occurred.</response>
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> PlaceOrder([FromBody] OrderCreateDto orderDto)
        {
            var userId = userService.GetCurentUserId();
            if (userId is null)
                return Unauthorized("User Id not found in the token");

            var serviceResult = await orderService.SubmitOrderAsync(userId.Value, orderDto);

            return StatusCode(serviceResult.StatusCode, serviceResult);
        }
        /// <summary>
        /// Retrieves specified order for the user.
        /// </summary>
        /// <param name="orderId">An Id of order to be retrieved </param>
        /// <returns>A service result containing retrieved order in data field.</returns>
        /// <response code="200">Order successfully retrieved..</response>
        /// <response code="400">Errors encountered during validation.</response>
        /// <response code="401">Invalid JWT token.</response>
        /// <response code="403">Order belogs to different user.</response>
        /// <response code="404">Non existent user or order Id.</response>
        /// <response code="500">An unexpected server-side error occurred.</response>
        [HttpGet("{orderId:guid}")]
        [Authorize]
        public async Task<IActionResult> GetOrderById(Guid orderId)
        {
            var userId = userService.GetCurentUserId();

            if (userId is null)
            {
                return StatusCode(400, ServiceResult<string>.Failure(
                    errors: ["User Id not found in the token"],
                    statusCode: 401));
            }

            var serviceResult = await orderService.GetOrderByIdAsync(userId.Value, orderId);

            return StatusCode(serviceResult.StatusCode, serviceResult);
        }

        /// <summary>
        /// Retrieves all orders for the user.
        /// </summary>
        /// <returns>A service result containing retrieved orders in data field.</returns>
        /// <response code="200">Orders successfully retrieved.</response>
        /// <response code="400">Errors encountered during validation.</response>
        /// <response code="401">Invalid JWT token.</response>
        /// <response code="403">Order belogs to different user.</response>
        /// <response code="404">Non existent user.</response>
        /// <response code="500">An unexpected server-side error occurred.</response>
        [HttpGet("user/all")]
        [Authorize]
        public async Task<IActionResult> GetOrderHistoryForUser()
        {
            var userId = userService.GetCurentUserId();
            if (userId is null)
                return Unauthorized("User Id not found in the token");

            var serviceResult = await orderService.GetAllOrdersForUserAsync(userId.Value);

            return StatusCode(serviceResult.StatusCode, serviceResult);
        }

        /// <summary>
        /// Cancels specified order submitted by the user.
        /// </summary>
        /// <param name="orderId">An id of order to be cancelled </param>
        /// <returns>A service result containing the cancelled order in data field.</returns>
        /// <response code="200">Orders successfully cancelled.</response>
        /// <response code="400">Order not in submitted state.</response>
        /// <response code="401">Invalid JWT token.</response>
        /// <response code="403">Order belogs to different user.</response>
        /// <response code="404">Non existent user or order.</response>
        /// <response code="500">An unexpected server-side error occurred.</response>
        [HttpPatch("{orderId:guid}/cancel")]
        [Authorize]
        public async Task<IActionResult> CancelOrder(Guid orderId)
        {
            var userId = userService.GetCurentUserId();
            if (userId is null)
                return Unauthorized("User Id not found in the token");

            var serviceResult = await orderService.CancelOrderAsync(userId.Value, orderId);

            return StatusCode(serviceResult.StatusCode, serviceResult);
        }
    }
}