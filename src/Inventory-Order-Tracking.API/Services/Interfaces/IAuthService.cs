using Inventory_Order_Tracking.API.Dtos;
using Inventory_Order_Tracking.API.Services.Shared;
using Inventory_Order_Tracking.API.Utils;

namespace Inventory_Order_Tracking.API.Services.Interfaces
{
    public interface IAuthService
    {
        Task<AuthServiceResult<string>> RegisterAsync(UserRegistrationDto request);

        Task<AuthServiceResult<TokenResponseDto>> LoginAsync(UserLoginDto request);

        Task<AuthServiceResult<TokenResponseDto>> RefreshTokens(RefreshTokenRequestDto request);
    }
}
