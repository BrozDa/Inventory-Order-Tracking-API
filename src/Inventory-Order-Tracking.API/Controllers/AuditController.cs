using Inventory_Order_Tracking.API.Domain;
using Inventory_Order_Tracking.API.Dtos;
using Inventory_Order_Tracking.API.Services.Interfaces;
using Inventory_Order_Tracking.API.Services.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Inventory_Order_Tracking.API.Controllers
{
    /// <summary>
    /// Controller responsible for fetching logs by admin users.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = UserRoles.Admin)]
    [Produces("application/json")]
    public class AuditController(
        IAuditLogService auditService) : ControllerBase
    {
        /// <summary>
        /// Retrieves all audit logs.
        /// </summary>
        /// <returns>A service result containing the audit logs in data field.</returns>
        /// <response code="200">Audit logs successfully retrieved.</response>
        /// <response code="401">Requesting user is not logged in.</response>
        /// <response code="403">Requesting user does not have admin role.</response>
        /// <response code="500">An unexpected server-side error occurred.</response>
        [HttpGet("all")]
        public async Task<IActionResult> GetAllLogs()
        {
            var serviceResult = await auditService.GetAllAsync();

            return StatusCode(serviceResult.StatusCode, serviceResult);
        }

        /// <summary>
        /// Retrieves all audit logs for provided User Id
        /// </summary>
        /// <param name="userId">An id of the user whose logs to be retrieved</param>
        /// <returns>A service result containing the filtered audit logs in data field.</returns>
        /// <response code="200">Audit logs successfully retrieved.</response>
        /// <response code="400">User Id validation failed.</response>
        /// <response code="401">Requesting user is not logged in.</response>
        /// <response code="403">Requesting user does not have admin role.</response>
        /// <response code="500">An unexpected server-side error occurred.</response>
        [HttpGet("by-user/{userId:guid}")]
        public async Task<IActionResult> GetAllForUser(Guid userId)
        {
            var serviceResult = await auditService.GetAllForUserAsync(userId);

            return StatusCode(serviceResult.StatusCode, serviceResult);
        }

        /// <summary>
        /// Retrieves all audit logs for provided User Id
        /// </summary>
        /// <param name="date">A date for which logs to be retrieved</param>
        /// <returns>A service result containing the filtered audit logs in data field.</returns>
        /// <response code="200">Audit logs successfully retrieved.</response>
        /// <response code="400">Date validation failed.</response>
        /// <response code="401">Requesting user is not logged in.</response>
        /// <response code="403">Requesting user does not have admin role.</response>
        /// <response code="500">An unexpected server-side error occurred.</response>
        [HttpGet("by-date/{date:datetime}")]
        public async Task<IActionResult> GetAllForDate(DateTime date)
        {
            var serviceResult = await auditService.GetAllForDateAsync(date);

            return StatusCode(serviceResult.StatusCode, serviceResult);
        }
    }
}