using Inventory_Order_Tracking.API.Models;
using Inventory_Order_Tracking.API.Services.Interfaces;
using System.Security.Claims;

namespace Inventory_Order_Tracking.API.Services
{
    public class CurrentUserService(IHttpContextAccessor contextAccessor) : ICurrentUserService
    {

        public Guid? GetCurentUserId()
        {
            var userIdString = contextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

            return Guid.TryParse(userIdString, out var userId)
            ? userId
            : null;
        }
    }
    
}
