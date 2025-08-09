using Inventory_Order_Tracking.API.Dtos;
using Inventory_Order_Tracking.API.Services.Interfaces;
using Inventory_Order_Tracking.API.Services.Shared;
using Inventory_Order_Tracking.API.Utils;
using Microsoft.AspNetCore.Mvc;

namespace Inventory_Order_Tracking.API.Controllers
{
    /// <summary>
    /// Controller responsible for fetching Authentication operations.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class AuthController(
        RegisterRequestValidator validator,
        IAuthService authService,
        IEmailVerificationService emailService,
        ILogger<AuthController> logger) : ControllerBase
    {

        /// <summary>
        /// Registers new user and sends verification email.
        /// </summary>
        /// <param name="request">An username and password of the new user </param>
        /// <returns>A service result containing the success message in data field.</returns>
        /// <response code="200">User successfully registered.</response>
        /// <response code="400">Errors encountered during validation.</response>
        /// <response code="500">An unexpected server-side error occurred.</response>
        [HttpPost("register")]
        public async Task<IActionResult> Register(UserRegistrationDto request)
        {
            var validationResult = validator.Validate(request);

            if (!validationResult.IsValid)
            {
                logger.LogWarning("[AuthController][Register] Validation failed for {Username}. Encountered Errors: {Errors}",
                    request.Username,
                    string.Join("; ", validationResult.Errors.Select(e => e.ErrorMessage)));

                return StatusCode(400, ServiceResult<string>.Failure(
                    errors: validationResult.Errors.Select(e => e.ErrorMessage).ToList(), 
                    statusCode:400));

            }

            var serviceResult = await authService.RegisterAsync(request);

            return StatusCode(serviceResult.StatusCode, serviceResult);
        }

        /// <summary>
        /// Perform login of an user and responds with JTW and refresh tokens.
        /// </summary>
        /// <param name="request">An username and password of existing user </param>
        /// <returns>A service result containing JTW and refresh tokens in data field.</returns>
        /// <response code="200">User successfully logged-in.</response>
        /// <response code="400">Errors encountered during validation.</response>
        /// <response code="500">An unexpected server-side error occurred.</response>
        [HttpPost("login")]
        public async Task<IActionResult> Login(UserLoginDto request)
        {
            var serviceResult = await authService.LoginAsync(request);

            return StatusCode(serviceResult.StatusCode, serviceResult);
        }

        /// <summary>
        /// Perform email verification of an user.
        /// </summary>
        /// <returns>A service result containing the verification success message.</returns>
        /// <response code="200">Email (User) successfuly validated</response>
        /// <response code="400">Errors encountered during validation.</response>
        /// <response code="500">An unexpected server-side error occurred.</response>
        [HttpGet("user/verify/{tokenId:guid}")]
        public async Task<IActionResult> Verify(Guid tokenId)
        {
            var serviceResult = await emailService.VerifyEmailAsync(tokenId);

            return StatusCode(serviceResult.StatusCode, serviceResult);
        }
    }
}