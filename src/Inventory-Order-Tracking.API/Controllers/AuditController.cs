using Inventory_Order_Tracking.API.Domain;
using Inventory_Order_Tracking.API.Dtos;
using Inventory_Order_Tracking.API.Services.Interfaces;
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
    public class AuditController(
        IAuditLogService auditService) : ControllerBase
    {
        /// <summary>
        /// Retrieves all audit logs.
        /// </summary>
        /// <returns>
        /// An OK <see cref="IActionResult"/> containing a list of <see cref="AuditLogDto"/> on success.
        /// Returns an appropriate status code and error message on failure.
        /// </returns>
        [HttpGet("all")]
        public async Task<IActionResult> GetAllLogs()
        {
            var serviceResult = await auditService.GetAllAsync();

            return serviceResult.IsSuccessful
            ? Ok(serviceResult.Data)
                : StatusCode((int)serviceResult.StatusCode, serviceResult.ErrorMessage);
        }

        /// <summary>
        /// Retrieves all audit logs for provided User Id
        /// </summary>
        /// <returns>
        /// An OK <see cref="IActionResult"/> containing a list of <see cref="AuditLogDto"/> on success.
        /// Returns an appropriate status code and error message on failure.
        /// </returns>
        [HttpGet("by-user")]
        public async Task<IActionResult> GetAllForUser([FromQuery] Guid userId)
        {
            var serviceResult = await auditService.GetAllForUserAsync(userId);

            return serviceResult.IsSuccessful
            ? Ok(serviceResult.Data)
                : StatusCode((int)serviceResult.StatusCode, serviceResult.ErrorMessage);
        }

        /// <summary>
        /// Retrieves all audit logs for provided Date
        /// </summary>
        /// <returns>
        /// An OK <see cref="IActionResult"/> containing a list of <see cref="AuditLogDto"/> on success.
        /// Returns an appropriate status code and error message on failure.
        /// </returns>
        [HttpGet("by-date")]
        public async Task<IActionResult> GetAllForDate([FromQuery] DateTime date)
        {
            var serviceResult = await auditService.GetAllForDateAsync(date);

            return serviceResult.IsSuccessful
            ? Ok(serviceResult.Data)
                : StatusCode((int)serviceResult.StatusCode, serviceResult.ErrorMessage);
        }
    }
}