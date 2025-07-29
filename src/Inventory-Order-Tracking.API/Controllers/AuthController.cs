using Inventory_Order_Tracking.API.Dtos;
using Inventory_Order_Tracking.API.Services;
using Inventory_Order_Tracking.API.Services.Interfaces;
using Inventory_Order_Tracking.API.Utils;
using Microsoft.AspNetCore.Mvc;


namespace Inventory_Order_Tracking.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController(RegisterRequestValidator validator, IAuthService service, ILogger<AuthController> logger) : ControllerBase
    {
        [HttpPost("register")]
        public async Task<IActionResult> Register(UserRegistrationDto request)
        {

            var validationResult = validator.Validate(request);

            if (!validationResult.IsValid) 
            {
                var errors = validationResult.Errors.Select(e => e.ErrorMessage);
                logger.LogInformation("[Register][Validation] Validation failed for {Username}. Encountered Errors: {Errors}",
                    request.Username,
                    errors);
                    
                return BadRequest(errors);
            }

            var serviceResult = await service.Register(request);

            return serviceResult.IsSuccessful 
                ? Ok(serviceResult.Data)
                : StatusCode((int)serviceResult.StatusCode, serviceResult.ErrorMessage);

        }
        [HttpPost("login")]
        public async Task<IActionResult> Login(UserLoginDto request) 
        {
            var serviceResult = await service.LoginAsync(request);

            return serviceResult.IsSuccessful
                ? Ok(serviceResult.Data)
                : StatusCode((int)serviceResult.StatusCode, serviceResult.ErrorMessage);
        }
        
    }
}
