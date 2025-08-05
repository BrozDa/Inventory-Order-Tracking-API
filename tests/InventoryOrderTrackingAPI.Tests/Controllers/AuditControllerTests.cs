using Inventory_Order_Tracking.API.Controllers;
using Inventory_Order_Tracking.API.Dtos;
using Inventory_Order_Tracking.API.Services.Interfaces;
using Inventory_Order_Tracking.API.Services.Shared;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryManagement.API.Tests.Controllers
{
    public class AuditControllerTests
    {
        private readonly AuditController _sut;
        private readonly Mock<IAuditService> _auditServiceMock = new();
        public AuditControllerTests()
        {
            _sut = new AuditController(_auditServiceMock.Object);
        }
        [Fact]
        public async Task GetAllLogs_FailedRequest_ReturnsOtherThanOK() 
        {
            //arrange
            var serviceResult = ServiceResult<List<AuditLogDto>>.BadRequest("Test failure");
            _auditServiceMock.Setup(s => s.GetAllAsync()).ReturnsAsync(serviceResult);

            //act
            var result = await _sut.GetAllLogs();
            //assert
            Assert.IsNotType<OkObjectResult>(result);
        }
        [Fact]
        public async Task GetAllLogs_SuccessfulRequest_ReturnsCreated()
        {
            //arrange
            var logs = new List<AuditLogDto>();
            var serviceResult = ServiceResult<List<AuditLogDto>>.Ok(logs);
            _auditServiceMock.Setup(s => s.GetAllAsync()).ReturnsAsync(serviceResult);

            //act
            var result = await _sut.GetAllLogs();
            //assert
            Assert.IsType<OkObjectResult>(result);
        }
        [Fact]
        public async Task GetAllForUser_FailedRequest_ReturnsOtherThanOK()
        {
            //arrange
            var userId = Guid.NewGuid();
            var serviceResult = ServiceResult<List<AuditLogDto>>.BadRequest("Test failure");

            _auditServiceMock.Setup(s => s.GetAllForUserAsync(userId)).ReturnsAsync(serviceResult);

            //act
            var result = await _sut.GetAllForUser(userId);
            //assert
            Assert.IsNotType<OkObjectResult>(result);
        }
        [Fact]
        public async Task GetAllForUser_SuccessfulRequest_ReturnsCreated()
        {
            //arrange
            var userId = Guid.NewGuid();
            var logs = new List<AuditLogDto>();
            var serviceResult = ServiceResult<List<AuditLogDto>>.Ok(logs);

            _auditServiceMock.Setup(s => s.GetAllForUserAsync(userId)).ReturnsAsync(serviceResult);

            //act
            var result = await _sut.GetAllForUser(userId);
            //assert
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task GetAllForDate_FailedRequest_ReturnsOtherThanOK()
        {
            //arrange
            var date = DateTime.UtcNow;
            var serviceResult = ServiceResult<List<AuditLogDto>>.BadRequest("Test failure");

            _auditServiceMock.Setup(s => s.GetAllForDateAsync(date)).ReturnsAsync(serviceResult);

            //act
            var result = await _sut.GetAllForDate(date);
            //assert
            Assert.IsNotType<OkObjectResult>(result);
        }
        [Fact]
        public async Task GetAllForDate_SuccessfulRequest_ReturnsCreated()
        {
            //arrange
            var date = DateTime.UtcNow;
            var logs = new List<AuditLogDto>();
            var serviceResult = ServiceResult<List<AuditLogDto>>.Ok(logs);

            _auditServiceMock.Setup(s => s.GetAllForDateAsync(date)).ReturnsAsync(serviceResult);

            //act
            var result = await _sut.GetAllForDate(date);
            //assert
            Assert.IsType<OkObjectResult>(result);
        }

    }
}
