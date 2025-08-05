using Inventory_Order_Tracking.API.Dtos;
using Inventory_Order_Tracking.API.Services.Shared;

namespace Inventory_Order_Tracking.API.Services.Interfaces
{
    public interface IAuthService
    {
        Task<ServiceResult<string>> RegisterAsync(UserRegistrationDto request);

        Task<ServiceResult<TokenResponseDto>> LoginAsync(UserLoginDto request);

        Task<ServiceResult<TokenResponseDto>> RefreshTokens(RefreshTokenRequestDto request);
    }
}