using Inventory_Order_Tracking.API.Controllers;
using Inventory_Order_Tracking.API.Dtos;
using Inventory_Order_Tracking.API.Services.Interfaces;
using Inventory_Order_Tracking.API.Services.Shared;
using Inventory_Order_Tracking.API.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
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

        var serviceResult = ServiceResult<string>.Failure(
                errors: ["Invalid inputs"],
                statusCode: 400);

        _authServiceMock.Setup(
            s => s.RegisterAsync(request))
            .ReturnsAsync(serviceResult);
        //act
        var result = await _sut.Register(request) as ObjectResult;

        //assert
        Assert.NotNull(result);
        Assert.Equal(400, result.StatusCode);
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

        var serviceResult = ServiceResult<string>.Success(
                message: "Registration successfull",
                statusCode: 200);

        _authServiceMock.Setup(
            s => s.RegisterAsync(request))
            .ReturnsAsync(serviceResult);
        //act
        var result = await _sut.Register(request) as ObjectResult;

        //assert
        Assert.NotNull(result);
        Assert.Equal(200, result.StatusCode);
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

        var serviceResult = ServiceResult<TokenResponseDto>.Failure(
                errors: ["Invalid Login"],
                statusCode: 400);

        _authServiceMock.Setup(
            s => s.LoginAsync(request))
            .ReturnsAsync(serviceResult);
        //act
        var result = await _sut.Login(request) as ObjectResult;
        //assert
        Assert.NotNull(result);
        Assert.Equal(400, result.StatusCode);
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

        var tokenResponseDto = new TokenResponseDto
        {
            AccessToken = "Sample Token",
            RefreshToken = "Refresh token"
        };
        var serviceResult = ServiceResult<TokenResponseDto>.Success(
                data: tokenResponseDto,
                statusCode: 200);

        _authServiceMock.Setup(
            s => s.LoginAsync(request))
            .ReturnsAsync(serviceResult);
        //act
        var result = await _sut.Login(request) as ObjectResult;
        //assert
        Assert.NotNull(result);
        Assert.Equal(200, result.StatusCode);
    }

    [Fact]
    public async Task Verify_ValidInput_ReturnsOk()
    {
        //arrange
        var serviceResult = ServiceResult<object>.Success(statusCode: 200);

        _emailVerificationServiceMock.Setup(x => x.VerifyEmailAsync(It.IsAny<Guid>())).ReturnsAsync(serviceResult);
        //act

        var result = await _sut.Verify(Guid.NewGuid()) as ObjectResult;

        //assert
        Assert.NotNull(result);
        Assert.Equal(200, result.StatusCode);
    }

    [Fact]
    public async Task Verify_InvalidInput_ReturnsOtherThan()
    {
        //arrange
        var serviceResult = ServiceResult<object>.Failure(
            errors: ["This is test generated error"],
            statusCode: 401);


        _emailVerificationServiceMock.Setup(x => x.VerifyEmailAsync(It.IsAny<Guid>())).ReturnsAsync(serviceResult);
        //act

        var result = await _sut.Verify(Guid.NewGuid()) as ObjectResult;

        //assert
        Assert.NotNull(result);
        Assert.Equal(401, result.StatusCode);
    }
}