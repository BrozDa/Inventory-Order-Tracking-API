using Inventory_Order_Tracking.API.Services.Interfaces;
using System.Security.Claims;

namespace Inventory_Order_Tracking.API.Services
{

    /// <summary>
    /// Provides functionality to retrieve information about the currently authenticated user from the HTTP context.
    /// </summary>
    public class CurrentUserService(IHttpContextAccessor contextAccessor) : ICurrentUserService
    {

        /// <inheritdoc/>
        public Guid? GetCurentUserId()
        {
            var userIdString = contextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

            return Guid.TryParse(userIdString, out var userId)
            ? userId
            : null;
        }
    }
    
}
