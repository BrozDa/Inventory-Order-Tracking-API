using Inventory_Order_Tracking.API.Controllers;
using Inventory_Order_Tracking.API.Dtos;
using Inventory_Order_Tracking.API.Services;
using Inventory_Order_Tracking.API.Services.Interfaces;
using Inventory_Order_Tracking.API.Services.Shared;
using Inventory_Order_Tracking.API.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System.Net;

namespace InventoryManagement.API.Tests;

public class AuthControllerTests
{
    private readonly AuthController _sut;
    private readonly RegisterRequestValidator _validator = new();
    private readonly Mock<IAuthService> _authServiceMock = new();
    private readonly Mock<ILogger<AuthController>> _loggerMock = new();
    private readonly Mock<IEmailVerificationService> _emailVerificationServiceMock = new();
    public AuthControllerTests()
    {
        _sut = new AuthController(_validator, _authServiceMock.Object, _emailVerificationServiceMock.Object, _loggerMock.Object);
    }

    [Fact]
    public async Task Register_InvalidInput_ReturnsBadRequest()
    {
        //arrange
        var request = new UserRegistrationDto
        {
            Username = "InvalidUsername",
            Password = "InvalidPassword",
            Email = "InvalidEmail"
        };

        _authServiceMock.Setup(
            s => s.RegisterAsync(request))
            .ReturnsAsync(AuthServiceResult<string>.BadRequest("Invalid inputs"));
        //act
        var result = await _sut.Register(request);

        //assert
        var badResult = Assert.IsType<BadRequestObjectResult>(result);
    }
    [Fact]
    public async Task Register_ValidInput_ReturnsOk()
    {
        //arrange
        var request = new UserRegistrationDto
        {
            Username = "ValidUsername",
            Password = "V4lidPassword",
            Email = "valid@email.com"
        };

        _authServiceMock.Setup(
            s => s.RegisterAsync(request))
            .ReturnsAsync(AuthServiceResult<string>.Ok("Registration successfull"));
        //act
        var result = await _sut.Register(request);

        //assert
        var okResult = Assert.IsType<OkObjectResult>(result);
    }
    [Fact]
    public async Task Login_InvalidInput_ReturnsBadRequest()
    {
        //arrange
        var request = new UserLoginDto
        {
            Username = "InvalidUsername",
            Password = "InvalidPass",
        };

        _authServiceMock.Setup(
            s => s.LoginAsync(request))
            .ReturnsAsync(AuthServiceResult<TokenResponseDto>.BadRequest("Invalid Login"));
        //act
        var result = await _sut.Login(request);
        //assert
        var badResult = Assert.IsType<ObjectResult>(result);
        Assert.Equal(badResult.StatusCode, 400);
    }
    [Fact]
    public async Task Login_ValidInput_ReturnsOk()
    {
        //arrange
        var request = new UserLoginDto
        {
            Username = "ValidUsername",
            Password = "V4lidPassword"
        };

        _authServiceMock.Setup(
            s => s.LoginAsync(request))
            .ReturnsAsync(AuthServiceResult<TokenResponseDto>
            .Ok(new TokenResponseDto 
                { 
                    AccessToken="Sample Token",
                    RefreshToken="Refresh token"
                }
            ));
        //act
        var result = await _sut.Login(request);
        //assert
        var okResult = Assert.IsType<OkObjectResult>(result);
    }
    [Fact]
    public async Task Verify_InvalidInput_ReturnsOtherThanOk() 
    {
        //arrange 
        var serviceResult = new EmailVerificationServiceResult
        {
            IsSuccessful = true,
            StatusCode = HttpStatusCode.OK
        };

        _emailVerificationServiceMock.Setup(x => x.VerifyEmail(It.IsAny<Guid>())).ReturnsAsync(serviceResult);
        //act

        var result = await _sut.Verify(Guid.NewGuid());

        //assert
        Assert.IsType<OkResult>(result);

    }
    [Fact]
    public async Task Verify_ValidInput_ReturnsOk() 
    {
        //arrange 
        var serviceResult = new EmailVerificationServiceResult
        { 
            IsSuccessful = false,
            ErrorMessage = "This is test generated error",
            StatusCode = HttpStatusCode.Unauthorized
        };

        _emailVerificationServiceMock.Setup(x => x.VerifyEmail(It.IsAny<Guid>())).ReturnsAsync(serviceResult);
        //act

        var result = await _sut.Verify(Guid.NewGuid());

        //assert
        Assert.IsNotType<OkResult>(result);

    }

}
