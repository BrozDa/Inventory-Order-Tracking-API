using Inventory_Order_Tracking.API.Dtos;
using Inventory_Order_Tracking.API.Services.Shared;

namespace Inventory_Order_Tracking.API.Services.Interfaces
{
    /// <summary>
    /// Defines a contract for authentication related operations within the app.
    /// </summary>
    public interface IAuthService
    {
        /// <summary>
        /// Perform registration of the user, sends a verification email and adds him to the data storage
        /// </summary>
        /// <param name="request"> An <see cref="UserRegistrationDto"/> containing details for newly registered user</param>
        /// <returns>
        /// A <see cref="ServiceResult{T}"/> containing message confirming registration on success,
        /// or an appropriate error message on failure.
        /// </returns>
        Task<ServiceResult<string>> RegisterAsync(UserRegistrationDto request);

        /// <summary>
        /// Performs login operations - verification of credentials, generating JWT adn refresh tokens
        /// </summary>
        /// <param name="request"> An <see cref="UserLoginDto"/> containing credentials of an user</param>
        /// <returns>
        /// A <see cref="ServiceResult{T}"/> containing JWT adn refresh tokens in form of <see cref="TokenResponseDto"/> objects on success,
        /// or an appropriate error message on failure.
        /// </returns>
        Task<ServiceResult<TokenResponseDto>> LoginAsync(UserLoginDto request);

        /// <summary>
        /// Performs validation and refresh of JWT and refresh tokens for the user
        /// </summary>
        /// <param name="request"> An <see cref="RefreshTokenRequestDto"/> containing user Id and expired refresh token</param>
        /// <returns>
        /// A <see cref="ServiceResult{T}"/> containing JWT and refresh tokens in form of <see cref="TokenResponseDto"/> objects on success,
        /// or an appropriate error message on failure.
        /// </returns>
        Task<ServiceResult<TokenResponseDto>> RefreshTokens(RefreshTokenRequestDto request);
    }
}