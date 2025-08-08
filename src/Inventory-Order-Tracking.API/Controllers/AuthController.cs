using Inventory_Order_Tracking.API.Dtos;
using Inventory_Order_Tracking.API.Services.Interfaces;
using Inventory_Order_Tracking.API.Utils;
using Microsoft.AspNetCore.Mvc;

namespace Inventory_Order_Tracking.API.Controllers
{
    /// <summary>
    /// Controller responsible for fetching Authentication operations.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController(
        RegisterRequestValidator validator,
        IAuthService authService,
        IEmailVerificationService emailService,
        ILogger<AuthController> logger) : ControllerBase
    {

        /// <summary>
        /// Perform registration for new user, adds him to database and sends verification email
        /// </summary>
        /// <param name="request">An <see cref="UserRegistrationDto"/> containing username and 
        /// password of new user 
        /// </param>
        /// <returns>
        /// An OK <see cref="IActionResult"/> containing success message on success.
        /// Returns an appropriate status code and error message on failure.
        /// </returns>
        [HttpPost("register")]
        public async Task<IActionResult> Register(UserRegistrationDto request)
        {
            var validationResult = validator.Validate(request);

            if (!validationResult.IsValid)
            {
                logger.LogWarning("[AuthController][Register] Validation failed for {Username}. Encountered Errors: {Errors}",
                    request.Username,
                    string.Join("; ", validationResult.Errors.Select(e => e.ErrorMessage)));

                return BadRequest(new { errors = validationResult.Errors.Select(e => e.ErrorMessage) });
            }

            var serviceResult = await authService.RegisterAsync(request);

            return serviceResult.IsSuccessful
                ? Ok(serviceResult.Data)
                : StatusCode((int)serviceResult.StatusCode, serviceResult.ErrorMessage);
        }

        /// <summary>
        /// Perform login of an user.
        /// </summary>
        /// <param name="request">An <see cref="UserRegistrationDto"/> containing username and 
        /// password of new user 
        /// </param>
        /// <returns>
        /// An OK <see cref="IActionResult"/> containing JWT token on success.
        /// Returns an appropriate status code and error message on failure.
        /// </returns>
        [HttpPost("login")]
        public async Task<IActionResult> Login(UserLoginDto request)
        {
            var serviceResult = await authService.LoginAsync(request);

            return serviceResult.IsSuccessful
                ? Ok(serviceResult.Data)
                : StatusCode((int)serviceResult.StatusCode, serviceResult.ErrorMessage);
        }

        /// <summary>
        /// Perform email verification of an user.
        /// </summary>
        /// <returns>
        /// An OK <see cref="IActionResult"/> on success.
        /// Returns an appropriate status code and error message on failure.
        /// </returns>
        [HttpGet("user/verify/{tokenId:guid}")]
        public async Task<IActionResult> Verify(Guid tokenId)
        {
            var serviceResult = await emailService.VerifyEmailAsync(tokenId);

            return serviceResult.IsSuccessful
                ? Ok()
                : StatusCode((int)serviceResult.StatusCode, serviceResult.ErrorMessage);
        }
    }
}