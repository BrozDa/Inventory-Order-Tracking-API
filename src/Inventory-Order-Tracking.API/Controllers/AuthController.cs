﻿using Inventory_Order_Tracking.API.Dtos;
using Inventory_Order_Tracking.API.Services.Interfaces;
using Inventory_Order_Tracking.API.Utils;
using Microsoft.AspNetCore.Mvc;


namespace Inventory_Order_Tracking.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController(
        RegisterRequestValidator validator,
        IAuthService authService,  
        IEmailVerificationService emailService,
        ILogger<AuthController> logger) : ControllerBase
    {
        [HttpPost("register")]
        public async Task<IActionResult> Register(UserRegistrationDto request)
        {

            var validationResult = validator.Validate(request);

            if (!validationResult.IsValid) 
            {
                logger.LogWarning("[Register][Validation] Validation failed for {Username}. Encountered Errors: {Errors}",
                    request.Username,
                    string.Join("; ", validationResult.Errors.Select(e => e.ErrorMessage)));
                    
                return BadRequest(new { errors = validationResult.Errors.Select(e => e.ErrorMessage) });
            }

            var serviceResult = await authService.RegisterAsync(request);

            return serviceResult.IsSuccessful 
                ? Ok(serviceResult.Data)
                : StatusCode((int)serviceResult.StatusCode, serviceResult.ErrorMessage);

        }
        [HttpPost("login")]
        public async Task<IActionResult> Login(UserLoginDto request) 
        {
            var serviceResult = await authService.LoginAsync(request);

            return serviceResult.IsSuccessful
                ? Ok(serviceResult.Data)
                : StatusCode((int)serviceResult.StatusCode, serviceResult.ErrorMessage);

        }

        [HttpGet("user/verify/{tokenId:guid}")]
        public async Task<IActionResult> Verify(Guid tokenId)
        {
            var serviceResult = await emailService.VerifyEmail(tokenId);

            return serviceResult.IsSuccessful
                ?  Ok()
                :  StatusCode((int)serviceResult.StatusCode, serviceResult.ErrorMessage);
        }
        
    }
}
