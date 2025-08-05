using FluentEmail.Core;
using FluentEmail.Core.Models;
using Inventory_Order_Tracking.API.Models;
using Inventory_Order_Tracking.API.Repository.Interfaces;
using Inventory_Order_Tracking.API.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging;
using Moq;
using System.Net;

namespace InventoryManagement.API.Tests.Services
{
    public class EmailVerificationServiceTests
    {
        private readonly EmailVerificationService _sut;
        private readonly Mock<IFluentEmail> _emailSenderMock = new();
        private readonly Mock<IEmailVerificationTokenRepository> _repoMock = new();
        private readonly Mock<IHttpContextAccessor> _httpContextMock = new();
        private readonly Mock<ILogger<EmailVerificationService>> _loggerMock = new();
        private readonly Mock<LinkGenerator> _linkGenerator = new();

        private string? _linkToReturn;

        public EmailVerificationServiceTests()
        {
            _sut = new EmailVerificationService(
                _emailSenderMock.Object,
                _repoMock.Object,
                _httpContextMock.Object,
                _loggerMock.Object,
                _linkGenerator.Object);
        }

        [Fact]
        public async Task SendVerificationEmail_TokenGenerationFails_ReturnsInternalServerError()
        { //reponse text ["Failed to generate verification token"]
            //arrange

            var userId = Guid.NewGuid();

            var user = new User
            {
                Id = userId,
                Role = "User",
                Username = "User",
                PasswordHash = "!Hash",
                PasswordSalt = "Salt=",
                Email = "test@email.com",
                IsVerified = false,
                RefreshToken = "token",
                RefreshTokenExpirationTime = DateTime.UtcNow.AddDays(1),
            };
            EmailVerificationToken token = new()
            {
                UserId = userId,
                CreatedOn = DateTime.UtcNow,
                ExpiresOn = DateTime.UtcNow.AddDays(1),
            };
            _repoMock.Setup(x => x.AddTokenAsync(It.IsAny<EmailVerificationToken>()))
                        .ReturnsAsync((EmailVerificationToken?)null);
            //act

            var result = await _sut.SendVerificationEmailAsync(user);
            //assert
            Assert.False(result.IsSuccessful);
            Assert.Equal(HttpStatusCode.InternalServerError, result.StatusCode);
            Assert.Equal("Could not store email verification token.", result.ErrorMessage);
        }

        [Fact]
        public async Task SendVerificationEmail_EmailSendFail_ReturnsInternalServerErrorAndLogs()
        { //reponse text ["Failed to sent verification email"]
            //arrange

            var userId = Guid.NewGuid();

            var user = new User
            {
                Id = userId,
                Role = "User",
                Username = "User",
                PasswordHash = "!Hash",
                PasswordSalt = "Salt=",
                Email = "test@email.com",
                IsVerified = false,
                RefreshToken = "token",
                RefreshTokenExpirationTime = DateTime.UtcNow.AddDays(1),
            };
            var token = new EmailVerificationToken
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                CreatedOn = DateTime.UtcNow,
                ExpiresOn = DateTime.UtcNow.AddDays(1),
                User = user
            };

            var sendResponse = new SendResponse
            {
                MessageId = "test-message-id",
                ErrorMessages = new List<string>() { "TestError" }
            };

            _emailSenderMock.Setup(x => x.To(It.IsAny<string>())).Returns(_emailSenderMock.Object);
            _emailSenderMock.Setup(x => x.Subject(It.IsAny<string>())).Returns(_emailSenderMock.Object);
            _emailSenderMock.Setup(x => x.Body(It.IsAny<string>(), It.IsAny<bool>())).Returns(_emailSenderMock.Object);
            _emailSenderMock.Setup(x => x.SendAsync(null)).ReturnsAsync(sendResponse);

            _repoMock.Setup(x => x.AddTokenAsync(It.IsAny<EmailVerificationToken>())).ReturnsAsync(token);
            //act

            var result = await _sut.SendVerificationEmailAsync(user);
            //assert
            Assert.False(result.IsSuccessful);
            Assert.Equal(HttpStatusCode.InternalServerError, result.StatusCode);
            Assert.Equal("Failed to sent verification email", result.ErrorMessage);
        }

        [Fact]
        public async Task SendVerificationEmail_UnhandledException_ReturnsInternalServerErrorAndLogs()
        { //reponse text ["Unhandled error occurred"]
            //arrange

            var userId = Guid.NewGuid();

            var token = new EmailVerificationToken
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                CreatedOn = DateTime.UtcNow,
                ExpiresOn = DateTime.UtcNow.AddDays(1),
                User = null
            };
            var user = new User
            {
                Id = userId,
                Role = "User",
                Username = "User",
                PasswordHash = "!Hash",
                PasswordSalt = "Salt=",
                Email = "test@email.com",
                IsVerified = false,
                RefreshToken = "token",
                RefreshTokenExpirationTime = DateTime.UtcNow.AddDays(1),
            };

            var sendResponse = new SendResponse
            {
                MessageId = "test-message-id",
                ErrorMessages = new List<string>() { }
            };

            _emailSenderMock.Setup(x => x.To(It.IsAny<string>())).Returns(_emailSenderMock.Object);
            _emailSenderMock.Setup(x => x.Subject(It.IsAny<string>())).Returns(_emailSenderMock.Object);
            _emailSenderMock.Setup(x => x.Body(It.IsAny<string>(), It.IsAny<bool>())).Returns(_emailSenderMock.Object);

            _emailSenderMock.Setup(x => x.SendAsync(null)).Throws<ArgumentNullException>();

            _repoMock.Setup(x => x.AddTokenAsync(It.IsAny<EmailVerificationToken>())).ReturnsAsync(token);
            //act

            var result = await _sut.SendVerificationEmailAsync(user);
            //assert
            Assert.False(result.IsSuccessful);
            Assert.Equal(HttpStatusCode.InternalServerError, result.StatusCode);
            Assert.Equal("Unhandled error occurred", result.ErrorMessage);
        }

        [Fact]
        public async Task SendVerificationEmail_ValidFlow_ReturnsOk()
        {
            //arrange

            var userId = Guid.NewGuid();

            var user = new User
            {
                Id = userId,
                Role = "User",
                Username = "User",
                PasswordHash = "!Hash",
                PasswordSalt = "Salt=",
                Email = "test@email.com",
                IsVerified = false,
                RefreshToken = "token",
                RefreshTokenExpirationTime = DateTime.UtcNow.AddDays(1),
            };

            var token = new EmailVerificationToken
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                CreatedOn = DateTime.UtcNow,
                ExpiresOn = DateTime.UtcNow.AddDays(1),
                User = user
            };

            var sendResponse = new SendResponse
            {
                MessageId = "test-message-id",
                ErrorMessages = new List<string>() { }
            };

            _emailSenderMock.Setup(x => x.To(It.IsAny<string>())).Returns(_emailSenderMock.Object);
            _emailSenderMock.Setup(x => x.Subject(It.IsAny<string>())).Returns(_emailSenderMock.Object);
            _emailSenderMock.Setup(x => x.Body(It.IsAny<string>(), It.IsAny<bool>())).Returns(_emailSenderMock.Object);

            _emailSenderMock.Setup(x => x.SendAsync(null)).ReturnsAsync(sendResponse);

            _repoMock.Setup(x => x.AddTokenAsync(It.IsAny<EmailVerificationToken>())).ReturnsAsync(token);
            //act

            var result = await _sut.SendVerificationEmailAsync(user);
            //assert
            Assert.True(result.IsSuccessful);
            Assert.Equal(HttpStatusCode.OK, result.StatusCode);
        }

        [Fact]
        public async Task VerifyEmail_InvalidToken_ReturnsUnauthorizedAndLogs()
        {//reponse text ["Invalid Token received"]
            //arrange
            var id = Guid.NewGuid();
            _repoMock.Setup(x => x.GetByIdAsync(id)).ReturnsAsync(default(EmailVerificationToken));
            //act
            var result = await _sut.VerifyEmailAsync(id);
            //assert

            Assert.False(result.IsSuccessful);
            Assert.Equal("Invalid Token received", result.ErrorMessage);
            Assert.Equal(HttpStatusCode.Unauthorized, result.StatusCode);

            _loggerMock.Verify(
                x => x.Log(
                LogLevel.Warning,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, _) => v.ToString().Contains("Invalid token verification attempt")),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
                Times.Once);
        }

        [Fact]
        public async Task VerifyEmail_TokenExpired_ReturnsUnauthorizedAndLogs()
        {//reponse text ["Verification link expired"]
         //arrange
            var id = Guid.NewGuid();
            var user = new User
            {
                Id = Guid.NewGuid(),
                Role = "User",
                Username = "User",
                PasswordHash = "!Hash",
                PasswordSalt = "Salt=",
                Email = "test@email.com",
                IsVerified = false,
                RefreshToken = "expiredToken",
                RefreshTokenExpirationTime = DateTime.UtcNow.AddDays(1),
            };
            var expiredToken = new EmailVerificationToken
            {
                Id = id,
                UserId = Guid.NewGuid(),
                CreatedOn = DateTime.UtcNow.AddDays(-5),
                ExpiresOn = DateTime.UtcNow.AddDays(-5),
                User = user,
            };

            _repoMock.Setup(x => x.GetByIdAsync(id)).ReturnsAsync(expiredToken);

            //act
            var result = await _sut.VerifyEmailAsync(id);

            //assert
            Assert.False(result.IsSuccessful);
            Assert.Equal("Verification link expired", result.ErrorMessage);
            Assert.Equal(HttpStatusCode.Unauthorized, result.StatusCode);

            _loggerMock.Verify(
                x => x.Log(
                LogLevel.Warning,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, _) => v.ToString().Contains("[EmailVerificationService][VerifyEmailAsync] Expired token verification attempt")),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
                Times.Once);
        }

        [Fact]
        public async Task VerifyEmail_UserAlreadyVerified_ReturnsUnauthorizedAndLogs()
        {//reponse text ["User already verified"]
            //arrange
            var id = Guid.NewGuid();
            var user = new User
            {
                Id = Guid.NewGuid(),
                Role = "User",
                Username = "User",
                PasswordHash = "!Hash",
                PasswordSalt = "Salt=",
                Email = "test@email.com",
                IsVerified = true,
                RefreshToken = "expiredToken",
                RefreshTokenExpirationTime = DateTime.UtcNow.AddDays(1),
            };
            var token = new EmailVerificationToken
            {
                Id = id,
                UserId = Guid.NewGuid(),
                CreatedOn = DateTime.UtcNow,
                ExpiresOn = DateTime.UtcNow.AddDays(1),
                User = user,
            };

            _repoMock.Setup(x => x.GetByIdAsync(id)).ReturnsAsync(token);

            //act
            var result = await _sut.VerifyEmailAsync(id);

            //assert
            Assert.False(result.IsSuccessful);
            Assert.Equal("User already verified", result.ErrorMessage);
            Assert.Equal(HttpStatusCode.Unauthorized, result.StatusCode);

            _loggerMock.Verify(
                x => x.Log(
                LogLevel.Warning,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, _) => v.ToString().Contains("[EmailVerificationService][VerifyEmailAsync] Already verified user attempt")),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
                Times.Once);
        }

        [Fact]
        public async Task VerifyEmail_UnhandledException_ReturnsInternalServerErrorAndLogs()
        { //reponse text ["Unhandled error occurred"]
            //arrange
            var id = Guid.NewGuid();
            var token = new EmailVerificationToken
            {
                Id = id,
                UserId = Guid.NewGuid(),
                CreatedOn = DateTime.UtcNow,
                ExpiresOn = DateTime.UtcNow.AddDays(1),
                User = null, // <<<<<<<<<<<<<<<<<<<<<<<<< Throws arg null exception upon checking expiration or verification
            };

            _repoMock.Setup(x => x.GetByIdAsync(id)).ReturnsAsync(token);
            //act
            var result = await _sut.VerifyEmailAsync(id);

            //assert
            Assert.False(result.IsSuccessful);
            Assert.Equal("Unhandled error occurred", result.ErrorMessage);
            Assert.Equal(HttpStatusCode.InternalServerError, result.StatusCode);

            _loggerMock.Verify(
                x => x.Log(
                LogLevel.Error,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, _) => v.ToString().Contains("[EmailVerificationService][VerifyEmailAsync] Unhandled error occurred")),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
                Times.Once);
        }

        [Fact]
        public async Task VerifyEmail_ValidFlow_ReturnsOk()
        {
            //arrange
            var tokenId = Guid.NewGuid();

            var user = new User
            {
                Id = Guid.NewGuid(),
                Role = "User",
                Username = "User",
                PasswordHash = "!Hash",
                PasswordSalt = "Salt=",
                Email = "test@email.com",
                IsVerified = false,
                RefreshToken = "token",
                RefreshTokenExpirationTime = DateTime.UtcNow.AddDays(1),
            };
            var token = new EmailVerificationToken
            {
                Id = tokenId,
                UserId = Guid.NewGuid(),
                CreatedOn = DateTime.UtcNow,
                ExpiresOn = DateTime.UtcNow.AddDays(1),
                User = user,
            };

            _repoMock.Setup(x => x.GetByIdAsync(tokenId)).ReturnsAsync(token);

            //act
            var result = await _sut.VerifyEmailAsync(tokenId);

            //assert
            Assert.True(result.IsSuccessful);
            Assert.Equal(HttpStatusCode.OK, result.StatusCode);
        }
    }
}