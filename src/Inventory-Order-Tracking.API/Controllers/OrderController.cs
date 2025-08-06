using Inventory_Order_Tracking.API.Dtos;
using Inventory_Order_Tracking.API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Inventory_Order_Tracking.API.Controllers
{

    /// <summary>
    /// Controller responsible for order handling operations.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController(
        ICurrentUserService userService,
        IOrderService orderService) :ControllerBase
    {

        /// <summary>
        /// Submits new order for an user.
        /// </summary>
        /// <param name="orderDto">An <see cref="CreateOrderDto"/> containing list of <see cref="OrderItemDto"/> 
        /// representing ordered items   
        /// </param>
        /// <returns>
        /// An Created <see cref="IActionResult"/> containing information about submitted order on success.
        /// Returns an appropriate status code and error message on failure.
        /// </returns>
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> PlaceOrder([FromBody] CreateOrderDto orderDto)
        {
            var userId = userService.GetCurentUserId();
            if (userId is null)
                return Unauthorized("User Id not found in the token");

            var serviceResult = await orderService.SubmitOrderAsync(userId.Value, orderDto);

            return serviceResult.IsSuccessful
            ? CreatedAtAction(nameof(GetOrderById), new { id = serviceResult.Data!.Id }, serviceResult.Data)
                : StatusCode((int)serviceResult.StatusCode, serviceResult.ErrorMessage);
        }

        /// <summary>
        /// Retrieves specified order for the user.
        /// </summary>
        /// <param name="orderId">An <see cref="Guid"/> or order to be retrieved 
        /// </param>
        /// <returns>
        /// An OK <see cref="IActionResult"/> containing information requested order on success.
        /// Returns an appropriate status code and error message on failure.
        /// </returns>
        [HttpGet("{orderId:guid}")]
        [Authorize]
        public async Task<IActionResult> GetOrderById(Guid orderId)
        {
            var userId = userService.GetCurentUserId();
            if (userId is null)
                return Unauthorized("User Id not found in the token");

            var serviceResult = await orderService.GetOrderByIdAsync(userId.Value, orderId);

            return serviceResult.IsSuccessful
            ? Ok(serviceResult.Data)
                : StatusCode((int)serviceResult.StatusCode, serviceResult.ErrorMessage);
        }
        /// <summary>
        /// Retrieves specified all orders submitted by the user.
        /// </summary>
        /// <returns>
        /// An OK <see cref="IActionResult"/> containing a list of orders.
        /// Returns an appropriate status code and error message on failure.
        /// </returns>
        [HttpGet("user/all")]
        [Authorize] 
        public async Task<IActionResult> GetOrderHistoryForUser()
        {
            var userId = userService.GetCurentUserId();
            if (userId is null)
                return Unauthorized("User Id not found in the token");

            var serviceResult = await orderService.GetAllOrdersForUserAsync(userId.Value);

            return serviceResult.IsSuccessful
            ? Ok(serviceResult.Data)
                : StatusCode((int)serviceResult.StatusCode, serviceResult.ErrorMessage);
        }
        /// <summary>
        /// Cancels specified order submitted by the user.
        /// </summary>
        /// <param name="orderId">An <see cref="Guid"/> or order to be cancelled 
        /// </param>
        /// <returns>
        /// An OK <see cref="IActionResult"/> containing information about cancelled order.
        /// Returns an appropriate status code and error message on failure.
        /// </returns>
        [HttpPut("{orderId:guid}/cancel")]
        [Authorize] 
        public async Task<IActionResult> CancelOrder(Guid orderId)
        {
            var userId = userService.GetCurentUserId();
            if (userId is null)
                return Unauthorized("User Id not found in the token");

            var serviceResult = await orderService.CancelOrderAsync(userId.Value, orderId);

            return serviceResult.IsSuccessful
            ? Ok(serviceResult.Data)
                : StatusCode((int)serviceResult.StatusCode, serviceResult.ErrorMessage);

        }

    }
}
