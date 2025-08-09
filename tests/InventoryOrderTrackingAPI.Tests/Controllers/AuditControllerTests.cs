using Inventory_Order_Tracking.API.Controllers;
using Inventory_Order_Tracking.API.Dtos;
using Inventory_Order_Tracking.API.Services.Interfaces;
using Inventory_Order_Tracking.API.Services.Shared;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace InventoryManagement.API.Tests.Controllers
{
    public class AuditControllerTests
    {
        private readonly AuditController _sut;
        private readonly Mock<IAuditLogService> _auditServiceMock = new();

        public AuditControllerTests()
        {
            _sut = new AuditController(_auditServiceMock.Object);
        }

        [Fact]
        public async Task GetAllLogs_FailedRequest_ReturnsOtherThanOK()
        {
            //arrange
            var serviceResult = ServiceResult<List<AuditLogDto>>.Failure(
                        errors: ["Test failure"],
                        statusCode: 400);

            _auditServiceMock.Setup(s => s.GetAllAsync()).ReturnsAsync(serviceResult);

            //act
            var result = await _sut.GetAllLogs() as ObjectResult;
            //assert
            Assert.NotNull(result);
            Assert.NotEqual(200, result.StatusCode);
        }

        [Fact]
        public async Task GetAllLogs_SuccessfulRequest_ReturnsOk()
        {
            //arrange
            var logs = new List<AuditLogDto>();
            var serviceResult = ServiceResult<List<AuditLogDto>>.Success(data: logs);
            _auditServiceMock.Setup(s => s.GetAllAsync()).ReturnsAsync(serviceResult);

            //act
            var result = await _sut.GetAllLogs() as ObjectResult;
            //assert
            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);
        }

        [Fact]
        public async Task GetAllForUser_FailedRequest_ReturnsOtherThanOK()
        {
            //arrange
            var userId = Guid.NewGuid();
            var serviceResult = ServiceResult<List<AuditLogDto>>.Failure(
                        errors: ["Test failure"],
                        statusCode: 400);

            _auditServiceMock.Setup(s => s.GetAllForUserAsync(userId)).ReturnsAsync(serviceResult);

            //act
            var result = await _sut.GetAllForUser(userId) as ObjectResult;
            //assert
            Assert.NotNull(result);
            Assert.NotEqual(200, result.StatusCode);
        }

        [Fact]
        public async Task GetAllForUser_SuccessfulRequest_ReturnsOk()
        {
            //arrange
            var userId = Guid.NewGuid();
            var logs = new List<AuditLogDto>();
            var serviceResult = ServiceResult<List<AuditLogDto>>.Success(data: logs);

            _auditServiceMock.Setup(s => s.GetAllForUserAsync(userId)).ReturnsAsync(serviceResult);

            //act
            var result = await _sut.GetAllForUser(userId) as ObjectResult;
            //assert
            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);
        }

        [Fact]
        public async Task GetAllForDate_FailedRequest_ReturnsOtherThanOK()
        {
            //arrange
            var date = DateTime.UtcNow;
            var serviceResult = ServiceResult<List<AuditLogDto>>.Failure(
                        errors: ["Test failure"],
                        statusCode: 400);

            _auditServiceMock.Setup(s => s.GetAllForDateAsync(date)).ReturnsAsync(serviceResult);

            //act
            var result = await _sut.GetAllForDate(date) as ObjectResult;
            //assert
            Assert.NotNull(result);
            Assert.NotEqual(200, result.StatusCode);
        }

        [Fact]
        public async Task GetAllForDate_SuccessfulRequest_ReturnsOk()
        {
            //arrange
            var date = DateTime.UtcNow;
            var logs = new List<AuditLogDto>();
            var serviceResult = ServiceResult<List<AuditLogDto>>.Success(data: logs);

            _auditServiceMock.Setup(s => s.GetAllForDateAsync(date)).ReturnsAsync(serviceResult);

            //act
            var result = await _sut.GetAllForDate(date) as ObjectResult;
            //assert
            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);
        }
    }
}