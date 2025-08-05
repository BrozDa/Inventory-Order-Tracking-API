using Inventory_Order_Tracking.API.Configuration;
using Inventory_Order_Tracking.API.Dtos;
using Inventory_Order_Tracking.API.Models;
using Inventory_Order_Tracking.API.Repository.Interfaces;
using Inventory_Order_Tracking.API.Services;
using Inventory_Order_Tracking.API.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using System.Net;

namespace InventoryOrderTracking.API.Tests;

public class AuthServiceTests
{
    private readonly AuthService _sut;

    private readonly JwtSettings _jwtSettings = new()
    {
        Token = "ThisIsMySuperSecretApiTokenTwiceThisIsMySuperSecretApiToken",
        Audience = "InventoryOrderTracking.API.Tests",
        Issuer = "InventoryOrderTracking.API.Tests",
        TokenExpirationDays = 7,
        RefreshTokenExpirationDays = 1
    };

    private readonly Mock<IUserRepository> _userRepoMock = new();
    private readonly Mock<ILogger<AuthService>> _loggerMock = new();
    private readonly Mock<IEmailVerificationService> _emailServiceMock = new();
    private readonly Mock<IAuditLogService> _auditServiceMock = new();

    public AuthServiceTests()
    {
        _sut = new AuthService(_userRepoMock.Object, _emailServiceMock.Object, _auditServiceMock.Object, _loggerMock.Object, _jwtSettings);
    }

    [Fact]
    public async Task Register_UsernameAlreadyExists_ReturnsBadRequestAndLogsWarning()
    {
        //arrange
        var request = new UserRegistrationDto
        {
            Username = "TestUsername",
            Password = "Test Password",
            Email = "Test@Email.com"
        };

        _userRepoMock.Setup(
            repo => repo.UsernameExistsAsync(request.Username)).ReturnsAsync(true);

        // act
        var result = await _sut.RegisterAsync(request);

        //assert
        Assert.False(result.IsSuccessful);
        Assert.Equal("User Already Exists", result.ErrorMessage);
        Assert.Equal(HttpStatusCode.BadRequest, result.StatusCode);

        _loggerMock.Verify(
        x => x.Log(
            LogLevel.Warning,
            It.IsAny<EventId>(),
            It.Is<It.IsAnyType>((v, t) =>
                v.ToString().Contains("[Registration][UsernameExistsAsync]")),
            It.IsAny<ArgumentNullException>(),
            It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
        Times.Once);

        _auditServiceMock.Verify(
            s => s.AddNewLogAsync(It.IsAny<AuditLogAddDto>()),
            Times.Never);
    }

    [Fact]
    public async Task Register_PasswordEmpty_ReturnsBadRequestAndLogsError()
    {   //arrange
        var request = new UserRegistrationDto
        {
            Username = "TestUsername",
            Password = "",
            Email = "Test@Email.com"
        };
        _userRepoMock.Setup(r => r.UsernameExistsAsync(It.IsAny<string>()))
        .ReturnsAsync(false);

        //act

        var result = await _sut.RegisterAsync(request);

        //assert
        Assert.False(result.IsSuccessful);
        Assert.Equal("Password cannot be empty", result.ErrorMessage);
        Assert.Equal(HttpStatusCode.BadRequest, result.StatusCode);

        _auditServiceMock.Verify(
            s => s.AddNewLogAsync(It.IsAny<AuditLogAddDto>()),
            Times.Never);
    }

    [Fact]
    public async Task Register_DbFailure_ReturnsBadRequestAndLogsError()
    {
        //arrange
        var request = new UserRegistrationDto
        {
            Username = "TestUsername",
            Password = "StringPassword11",
            Email = "Test@email.com"
        };
        _userRepoMock.Setup(r => r.UsernameExistsAsync(It.IsAny<string>()))
        .ReturnsAsync(false);

        _userRepoMock.Setup(r => r.AddAsync(It.IsAny<User>()))
            .Throws(new DbUpdateException("Simulated error", new Exception("Inner exception")));
        //act
        var result = await _sut.RegisterAsync(request);

        //assert
        Assert.False(result.IsSuccessful);
        Assert.Equal("A database error occured during processing the request", result.ErrorMessage);
        Assert.Equal(HttpStatusCode.InternalServerError, result.StatusCode);

        _auditServiceMock.Verify(
            s => s.AddNewLogAsync(It.IsAny<AuditLogAddDto>()),
            Times.Never);
    }

    [Fact]
    public async Task Register_UnexpectedError_ReturnsBadRequestAndLogsError()
    {
        //arrange
        var request = new UserRegistrationDto
        {
            Username = "TestUsername",
            Password = "StringPassword11",
            Email = "Test@email.com"
        };
        _userRepoMock.Setup(r => r.UsernameExistsAsync(It.IsAny<string>()))
        .ReturnsAsync(false);

        _userRepoMock.Setup(r => r.AddAsync(It.IsAny<User>()))
            .Throws(new Exception("Unexpected exception"));
        //act
        var result = await _sut.RegisterAsync(request);

        //assert
        Assert.False(result.IsSuccessful);
        Assert.Equal("An Unexpected error occured during processing the request", result.ErrorMessage);
        Assert.Equal(HttpStatusCode.InternalServerError, result.StatusCode);

        _loggerMock.Verify(
        x => x.Log(
            LogLevel.Error,
            It.IsAny<EventId>(),
            It.Is<It.IsAnyType>((v, t) =>
                v.ToString().Contains("[Registration][UnhandledException]")),
            It.IsAny<Exception>(),
            It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
        Times.Once);

        _auditServiceMock.Verify(
            s => s.AddNewLogAsync(It.IsAny<AuditLogAddDto>()),
            Times.Never);
    }

    [Fact]
    public async Task Register_ValidInput_AddstoDbAndReturnsOk()
    {
        //arrange
        var request = new UserRegistrationDto
        {
            Username = "TestUsername",
            Password = "StringPassword11",
            Email = "Test@email.com"
        };
        _userRepoMock.Setup(
            r => r.AddAsync(It.IsAny<User>())).ReturnsAsync(new User());

        //act
        var result = await _sut.RegisterAsync(request);
        //assert
        Assert.True(result.IsSuccessful);
        Assert.Equal("Registration successful. Please verify your email to activate your account.", result.Data);
        Assert.Equal(HttpStatusCode.OK, result.StatusCode);

        _auditServiceMock.Verify(
            s => s.AddNewLogAsync(It.IsAny<AuditLogAddDto>()),
            Times.Once);
    }

    [Fact]
    public async Task Login_NonExistingUser_ReturnsBadRequestAndLogsWarning()
    {
        //arrange
        var request = new UserLoginDto
        {
            Username = "NonExistingUsername",
            Password = "StringPassword11",
        };

        _userRepoMock.Setup(
            r => r.GetByUsernameAsync(request.Username))
            .ReturnsAsync((User?)null);
        //act

        var result = await _sut.LoginAsync(request);

        //assert
        Assert.False(result.IsSuccessful);
        Assert.Equal("Invalid username or password", result.ErrorMessage);
        Assert.Equal(HttpStatusCode.BadRequest, result.StatusCode);

        _loggerMock.Verify(
        x => x.Log(
            LogLevel.Warning,
            It.IsAny<EventId>(),
            It.Is<It.IsAnyType>((v, t) =>
                v.ToString().Contains("[LoginAsync][AuthenticationFailed]")),
            null,
            It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
        Times.Once);
    }

    [Fact]
    public async Task Login_EmptyPassword_ReturnsBadRequesAndLogsWarning()
    {
        //arrange
        var request = new UserLoginDto
        {
            Username = "Username",
            Password = "",
        };
        var user = new User
        {
            Id = Guid.NewGuid(),
            Role = "User",
            Username = request.Username,
            PasswordHash = "!@##%$%NotCorrect",
            PasswordSalt = "AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA=",
            Email = "test@email.com",
            IsVerified = true
        };

        _userRepoMock.Setup(
            r => r.GetByUsernameAsync(request.Username))
            .ReturnsAsync(user);
        //act

        var result = await _sut.LoginAsync(request);

        //assert
        Assert.False(result.IsSuccessful);
        Assert.Equal("Invalid username or password", result.ErrorMessage);
        Assert.Equal(HttpStatusCode.BadRequest, result.StatusCode);

        _loggerMock.Verify(
        x => x.Log(
            LogLevel.Warning,
            It.IsAny<EventId>(),
            It.Is<It.IsAnyType>((v, t) =>
                v.ToString().Contains("[LoginAsync][AuthenticationFailed]")),
            null,
            It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
        Times.Once);
    }

    [Fact]
    public async Task Login_IncorrectPassword_ReturnsBadRequesAndLogsWarning()
    {
        //arrange
        var request = new UserLoginDto
        {
            Username = "Username",
            Password = "IncorrectPassword",
        };
        var user = new User
        {
            Id = Guid.NewGuid(),
            Role = "User",
            Username = request.Username,
            PasswordHash = "!@##%$%",
            PasswordSalt = "AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA=",
            Email = "test@email.com",
            IsVerified = true
        };

        _userRepoMock.Setup(
            r => r.GetByUsernameAsync(request.Username))
            .ReturnsAsync(user);
        //act

        var result = await _sut.LoginAsync(request);

        //assert
        Assert.False(result.IsSuccessful);
        Assert.Equal("Invalid username or password", result.ErrorMessage);
        Assert.Equal(HttpStatusCode.BadRequest, result.StatusCode);

        _loggerMock.Verify(
        x => x.Log(
            LogLevel.Warning,
            It.IsAny<EventId>(),
            It.Is<It.IsAnyType>((v, t) =>
                v.ToString().Contains("[LoginAsync][AuthenticationFailed]")),
            null,
            It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
        Times.Once);
    }

    [Fact]
    public async Task Login_CorrectCredentialsUnverifiedUser_ReturnsBadRequesAndLogsWarning()
    {
        //arrange
        var request = new UserLoginDto
        {
            Username = "string",
            Password = "String11",
        };
        var user = new User
        {
            Id = Guid.NewGuid(),
            Role = "User",
            Username = request.Username,
            PasswordHash = "kK8sQdxvGDbbJqljooXC1QFjuRON3xchEH2m76AgVJY=",
            PasswordSalt = "NuxupY+YTWig0beCC4Ve0CwIRGvBvM3NT2KxnWTyaZurw+bUy9e3mBNgkjZiwTggTa9gG8fwPPYCdps5ci0SlwQm0X7SvB/wvpg4m8PS4IqVGeHmeSPjYeuMlcQ0p0IAwwn/y8MGcVSAxs4hSQja7VSJNJFip9scrmOg/lMQoQk=",
            Email = "test@email.com",
            IsVerified = false
        };

        _userRepoMock.Setup(
            r => r.GetByUsernameAsync(request.Username))
            .ReturnsAsync(user);
        //act

        var result = await _sut.LoginAsync(request);

        //assert
        Assert.False(result.IsSuccessful);
        Assert.Equal("User not verified", result.ErrorMessage);
        Assert.Equal(HttpStatusCode.BadRequest, result.StatusCode);

        _loggerMock.Verify(
        x => x.Log(
            LogLevel.Warning,
            It.IsAny<EventId>(),
            It.Is<It.IsAnyType>((v, t) =>
                v.ToString().Contains("[LoginAsync][Unverified]")),
            null,
            It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
        Times.Once);
    }

    [Fact]
    public async Task Login_UnhandledException_ReturnsBadRequesAndLogsError()
    {
        //arrange
        var request = new UserLoginDto
        {
            Username = "Username",
            Password = "",
        };
        var user = new User
        {
            Id = Guid.NewGuid(),
            Role = "User",
            Username = request.Username,
            PasswordHash = "!@##%$%NotCorrect",
            PasswordSalt = "AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA=",
            Email = "test@email.com",
            IsVerified = true
        };

        _userRepoMock.Setup(
            r => r.GetByUsernameAsync(request.Username))
            .Throws(new Exception("Unhandled exception"));
        //act

        var result = await _sut.LoginAsync(request);

        //assert
        Assert.False(result.IsSuccessful);
        Assert.Equal("An Unexpected error occured during processing the request", result.ErrorMessage);
        Assert.Equal(HttpStatusCode.InternalServerError, result.StatusCode);

        _loggerMock.Verify(
        x => x.Log(
            LogLevel.Error,
            It.IsAny<EventId>(),
            It.Is<It.IsAnyType>((v, t) =>
                v.ToString().Contains("[LoginAsync][Exception]")),
            It.IsAny<Exception>(),
            It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
        Times.Once);
    }

    [Fact]
    public async Task RefreshTokens_InvalidUserId_ReturnUnauthorized()
    {
        var userId = Guid.NewGuid();
        var expiredToken = "ThisIsExpiredRefreshToken";
        var request = new RefreshTokenRequestDto
        {
            UserId = userId,
            ExpiredRefreshToken = expiredToken
        };

        _userRepoMock.Setup(x => x.GetByIdAsync(request.UserId)).ReturnsAsync(default(User));

        var result = await _sut.RefreshTokens(request);

        Assert.False(result.IsSuccessful);
        Assert.Equal(HttpStatusCode.Unauthorized, result.StatusCode);
    }

    [Fact]
    public async Task RefreshTokens_RefreshTokensDontMatch_ReturnsUnauthorized()
    {
        var userId = Guid.NewGuid();
        var expiredToken = "ThisIsExpiredRefreshToken";
        var request = new RefreshTokenRequestDto
        {
            UserId = userId,
            ExpiredRefreshToken = expiredToken
        };

        _userRepoMock
            .Setup(x => x.GetByIdAsync(request.UserId))
            .ReturnsAsync(new User
            {
                Id = userId,
                Role = "User",
                Username = "Test user",
                PasswordHash = "!@##%$%NotCorrect",
                PasswordSalt = "AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA=",
                Email = "test@email.com",
                IsVerified = true,
                RefreshToken = "DifferentExpiredToken",
                RefreshTokenExpirationTime = DateTime.UtcNow.AddDays(15),
            });

        var result = await _sut.RefreshTokens(request);

        Assert.False(result.IsSuccessful);
        Assert.Equal(HttpStatusCode.Unauthorized, result.StatusCode);
    }

    [Fact]
    public async Task RefreshTokens_RefreshTokenExpired_ReturnsUnauthorized()
    {
        var userId = Guid.NewGuid();
        var expiredToken = "ThisIsExpiredRefreshToken";
        var request = new RefreshTokenRequestDto
        {
            UserId = userId,
            ExpiredRefreshToken = expiredToken
        };

        _userRepoMock
            .Setup(x => x.GetByIdAsync(request.UserId))
            .ReturnsAsync(new User
            {
                Id = userId,
                Role = "User",
                Username = "Test user",
                PasswordHash = "!@##%$%NotCorrect",
                PasswordSalt = "AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA=",
                Email = "test@email.com",
                IsVerified = true,
                RefreshToken = "expiredToken",
                RefreshTokenExpirationTime = DateTime.UtcNow.AddDays(-15),
            });

        var result = await _sut.RefreshTokens(request);

        Assert.False(result.IsSuccessful);
        Assert.Equal(HttpStatusCode.Unauthorized, result.StatusCode);
    }

    [Fact]
    public async Task RefreshTokens_ValidInput_ReturnsTokenResponseDto()
    {
        var userId = Guid.NewGuid();
        var expiredToken = "expiredToken";
        var request = new RefreshTokenRequestDto
        {
            UserId = userId,
            ExpiredRefreshToken = expiredToken
        };

        _userRepoMock
            .Setup(x => x.GetByIdAsync(request.UserId))
            .ReturnsAsync(new User
            {
                Id = userId,
                Role = "User",
                Username = "Test user",
                PasswordHash = "!@##%$%Correct",
                PasswordSalt = "AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA=",
                Email = "test@email.com",
                IsVerified = true,
                RefreshToken = "expiredToken",
                RefreshTokenExpirationTime = DateTime.UtcNow.AddDays(1),
            });

        var result = await _sut.RefreshTokens(request);

        Assert.True(result.IsSuccessful);
        Assert.Equal(HttpStatusCode.OK, result.StatusCode);
    }
}