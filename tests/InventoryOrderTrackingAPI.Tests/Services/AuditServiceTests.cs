using Inventory_Order_Tracking.API.Dtos;
using Inventory_Order_Tracking.API.Models;
using Inventory_Order_Tracking.API.Repository.Interfaces;
using Inventory_Order_Tracking.API.Services;
using Microsoft.Extensions.Logging;
using Moq;
using System.Net;

namespace InventoryManagement.API.Tests.Services
{
    public class AuditServiceTests
    {
        private readonly AuditLogService _sut;
        private readonly Mock<IAuditLogRepository> _auditLogRepository = new();
        private readonly Mock<IUserRepository> _userRepository = new();
        private readonly Mock<ILogger<AuditLogService>> _loggerMock = new();

        public AuditServiceTests()
        {
            _sut = new(_auditLogRepository.Object, _userRepository.Object, _loggerMock.Object);
        }

        [Fact]
        public async Task GetAllAsync_RepoThrowsException_ReturnsInternalServerErrorAndLogsError()
        {
            //arrange

            _auditLogRepository.Setup(ar => ar.GetAllAuditLogsAsync()).Throws(() => new Exception("Test exception"));
            //act

            var result = await _sut.GetAllAsync();

            //assert
            Assert.False(result.IsSuccessful);
            Assert.Equal(500, result.StatusCode);
            Assert.Equal(["Failed to fetch logs"], result.Errors);

            _loggerMock.Verify(
                x => x.Log(
                LogLevel.Error,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, _) => v.ToString().Contains("[AuditService][GetAllAsync] Unhandled Exception has occured")),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
                Times.Once);
        }

        [Fact]
        public async Task GetAllAsync_ValidFlow_ReturnsOkWithLogs()
        {
            //arrange
            var logs = new List<AuditLog>();
            _auditLogRepository.Setup(ar => ar.GetAllAuditLogsAsync()).ReturnsAsync(logs);
            //act

            var result = await _sut.GetAllAsync();
            //assert
            Assert.True(result.IsSuccessful);
            Assert.Equal(200, result.StatusCode);
            Assert.NotNull(logs);
        }

        [Fact]
        public async Task GetAllForDate_RepoThrowsException_ReturnsInternalServerErrorAndLogsError()
        {
            //arrange
            var date = DateTime.UtcNow.AddDays(-2);
            _auditLogRepository.Setup(ar => ar.GetAllForDateAsync(date)).Throws(() => new Exception("Test exception"));
            //act

            var result = await _sut.GetAllForDateAsync(date);

            //assert
            Assert.False(result.IsSuccessful);
            Assert.Equal(500, result.StatusCode);
            Assert.Equal(["Failed to fetch logs"], result.Errors);

            _loggerMock.Verify(
                x => x.Log(
                LogLevel.Error,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, _) => v.ToString().Contains("[AuditService][GetAllForDateAsync] Unhandled Exception has occured")),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
                Times.Once);
        }

        [Fact]
        public async Task GetAllForDate_DateInFuture_ReturnBadRequestAndLogsWarning()
        {
            //arrange
            var date = DateTime.UtcNow.AddDays(2);
            var logs = new List<AuditLog>();
            _auditLogRepository.Setup(ar => ar.GetAllForDateAsync(date)).ReturnsAsync(logs);
            //act

            var result = await _sut.GetAllForDateAsync(date);

            //assert
            Assert.False(result.IsSuccessful);
            Assert.Equal(400, result.StatusCode);
            Assert.Equal(["Cannot get logs for future date"], result.Errors);

            _loggerMock.Verify(
                x => x.Log(
                LogLevel.Warning,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, _) => v.ToString().Contains("[AuditService][GetAllForDateAsync] Attempt to gather logs for future date")),
                null,
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
                Times.Once);
        }

        [Fact]
        public async Task GetAllForDate_SameDate_ReturnsOkWithLogs()
        {
            //arrange
            var date = DateTime.UtcNow;
            var logs = new List<AuditLog>();
            _auditLogRepository.Setup(ar => ar.GetAllForDateAsync(date)).ReturnsAsync(logs);
            //act

            var result = await _sut.GetAllForDateAsync(date);

            //assert
            Assert.True(result.IsSuccessful);
            Assert.Equal(200, result.StatusCode);
            Assert.NotNull(logs);
        }

        [Fact]
        public async Task GetAllForDate_PastDate_ReturnsOkWithLogs()
        {
            //arrange
            var date = DateTime.UtcNow.AddDays(-2);
            var logs = new List<AuditLog>();
            _auditLogRepository.Setup(ar => ar.GetAllForDateAsync(date)).ReturnsAsync(logs);
            //act

            var result = await _sut.GetAllForDateAsync(date);

            //assert
            Assert.True(result.IsSuccessful);
            Assert.Equal(200, result.StatusCode);
            Assert.NotNull(logs);
        }

        [Fact]
        public async Task GetAllForUser_RepoThrowsException_ReturnsInternalServerErrorAndLogsError()
        {
            //arrange
            var userId = Guid.NewGuid();

            _userRepository.Setup(ur => ur.IdExistsAsync(userId)).ReturnsAsync(true);
            _auditLogRepository.Setup(ar => ar.GetAllForUserAsync(userId)).Throws(() => new Exception("Test exception"));
            //act

            var result = await _sut.GetAllForUserAsync(userId);

            //assert
            Assert.False(result.IsSuccessful);
            Assert.Equal(500, result.StatusCode);
            Assert.Equal(["Failed to fetch logs"], result.Errors);

            _loggerMock.Verify(
                x => x.Log(
                LogLevel.Error,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, _) => v.ToString().Contains("[AuditService][GetAllForUserAsync] Unhandled Exception has occured")),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
                Times.Once);
        }

        [Fact]
        public async Task GetAllForUser_UserDoesNotExist_ReturnBadRequestAndLogsWarning()
        {
            //arrange
            var userId = Guid.NewGuid();
            var logs = new List<AuditLog>();

            _userRepository.Setup(ur => ur.IdExistsAsync(userId)).ReturnsAsync(false);
            _auditLogRepository.Setup(ar => ar.GetAllForUserAsync(userId)).ReturnsAsync(logs);

            //act
            var result = await _sut.GetAllForUserAsync(userId);

            //assert
            Assert.False(result.IsSuccessful);
            Assert.Equal(400, result.StatusCode);
            Assert.Equal(["User with provided Id does not exist"], result.Errors);

            _loggerMock.Verify(
                x => x.Log(
                LogLevel.Warning,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, _) => v.ToString().Contains("[AuditService][GetAllForUserAsync] Attempt to gather logs for non existing user")),
                null,
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
                Times.Once);
        }

        [Fact]
        public async Task GetAllForUser_ValidFlow_ReturnsOkWithLogs()
        {
            //arrange
            var userId = Guid.NewGuid();
            var logs = new List<AuditLog>();

            _userRepository.Setup(ur => ur.IdExistsAsync(userId)).ReturnsAsync(true);
            _auditLogRepository.Setup(ar => ar.GetAllForUserAsync(userId)).ReturnsAsync(logs);

            //act
            var result = await _sut.GetAllForUserAsync(userId);

            //assert
            Assert.True(result.IsSuccessful);
            Assert.Equal(200, result.StatusCode);
            Assert.NotNull(logs);
        }

        [Fact]
        public async Task AddNewLogAsync_RepoThrowsException_ReturnsInternalServerErrorAndLogsError()
        {
            //arrange
            var userId = Guid.NewGuid();

            var action = "Registered";

            var newLogDto = new AuditLogAddDto
            {
                UserId = userId,
                Action = action
            };

            var newLog = new AuditLog
            {
                UserId = userId,
                Action = action
            };
            _userRepository.Setup(ur => ur.IdExistsAsync(userId)).ReturnsAsync(true);
            _auditLogRepository.Setup(ar => ar.AddAsync(It.IsAny<AuditLog>())).Throws(() => new Exception("Test exception"));
            //act

            var result = await _sut.AddNewLogAsync(newLogDto);

            //assert
            Assert.False(result.IsSuccessful);
            Assert.Equal(500, result.StatusCode);
            Assert.Equal(["Failed to add new log"], result.Errors);

            _loggerMock.Verify(
                x => x.Log(
                LogLevel.Error,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, _) => v.ToString().Contains("[AuditService][AddNewLogAsync] Unhandled Exception has occured")),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
                Times.Once);
        }

        [Fact]
        public async Task AddNewLogAsync_UserDoesNotExist_ReturnBadRequestAndLogsWarning()
        {
            //arrange
            var userId = Guid.NewGuid();

            var action = "Registered";

            var newLogDto = new AuditLogAddDto
            {
                UserId = userId,
                Action = action
            };

            var newLog = new AuditLog
            {
                UserId = userId,
                Action = action
            };
            _userRepository.Setup(ur => ur.IdExistsAsync(userId)).ReturnsAsync(false);
            _auditLogRepository.Setup(ar => ar.AddAsync(It.IsAny<AuditLog>()));
            //act

            var result = await _sut.AddNewLogAsync(newLogDto);

            //assert
            Assert.False(result.IsSuccessful);
            Assert.Equal(400, result.StatusCode);
            Assert.Equal(["Cannot add logs for non existent user"], result.Errors);

            _loggerMock.Verify(
                x => x.Log(
                LogLevel.Warning,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, _) => v.ToString().Contains("[AuditService][AddNewLogAsync] Attempt to add log for non existent user")),
                null,
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
                Times.Once);
        }

        [Fact]
        public async Task AddNewLogAsync_ValidFlow_ReturnsCreated()
        {
            //arrange
            var userId = Guid.NewGuid();

            var action = "Registered";

            var newLogDto = new AuditLogAddDto
            {
                UserId = userId,
                Action = action
            };

            var newLog = new AuditLog
            {
                UserId = userId,
                Action = action
            };
            _userRepository.Setup(ur => ur.IdExistsAsync(userId)).ReturnsAsync(true);
            _auditLogRepository.Setup(ar => ar.AddAsync(It.IsAny<AuditLog>()));
            //act

            var result = await _sut.AddNewLogAsync(newLogDto);

            //assert
            Assert.True(result.IsSuccessful);
            Assert.Equal(201, result.StatusCode);
            Assert.NotNull(result.Data);
        }
    }
}