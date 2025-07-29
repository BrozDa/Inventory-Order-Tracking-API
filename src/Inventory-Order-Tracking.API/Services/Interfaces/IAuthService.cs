using Inventory_Order_Tracking.API.Dtos;
using Inventory_Order_Tracking.API.Services.Shared;
using Inventory_Order_Tracking.API.Utils;

namespace Inventory_Order_Tracking.API.Services.Interfaces
{
    public interface IAuthService
    {
        Task<AuthServiceResult<string>> Register(UserRegistrationDto request);

        Task<AuthServiceResult<string>> LoginAsync(UserLoginDto request);
    }
}
