using Inventory_Order_Tracking.API.Domain;
using Inventory_Order_Tracking.API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Inventory_Order_Tracking.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = UserRoles.Admin)]
    public class AuditController(
        IAuditLogService auditService) : ControllerBase
    {
        [HttpGet("all")]
        public async Task<IActionResult> GetAllLogs()
        {
            var serviceResult = await auditService.GetAllAsync();

            return serviceResult.IsSuccessful
            ? Ok(serviceResult.Data)
                : StatusCode((int)serviceResult.StatusCode, serviceResult.ErrorMessage);
        }
        [HttpGet("by-user")]
        public async Task<IActionResult> GetAllForUser([FromQuery] Guid userId)
        {
            var serviceResult = await auditService.GetAllForUserAsync(userId);

            return serviceResult.IsSuccessful
            ? Ok(serviceResult.Data)
                : StatusCode((int)serviceResult.StatusCode, serviceResult.ErrorMessage);
        }
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
