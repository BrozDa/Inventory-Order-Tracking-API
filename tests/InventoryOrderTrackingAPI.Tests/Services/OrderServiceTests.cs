using Castle.Core.Logging;
using Inventory_Order_Tracking.API.Domain;
using Inventory_Order_Tracking.API.Dtos;
using Inventory_Order_Tracking.API.Models;
using Inventory_Order_Tracking.API.Repository.Interfaces;
using Inventory_Order_Tracking.API.Services;
using Inventory_Order_Tracking.API.Services.Interfaces;
using Inventory_Order_Tracking.API.Services.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.SqlServer.Server;
using Moq;
using Serilog.Core;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace InventoryManagement.API.Tests.Services
{
    

    public class OrderServiceTests
    {
        public readonly OrderService _sut;
        public readonly Mock<IAuditService> _auditServiceMock = new();
        public readonly Mock<IUserRepository> _userRepositoryMock = new();
        public readonly Mock<IOrderRepository> _orderRepositoryMock = new();
        public readonly Mock<IProductRepository> _productRepositoryMock = new();
        private readonly Mock<ILogger<OrderService>> _loggerMock = new();


        public OrderServiceTests()
        {
            _sut = new(_auditServiceMock.Object,
                _userRepositoryMock.Object,
                _productRepositoryMock.Object,
                _orderRepositoryMock.Object,
                _loggerMock.Object);
        }

        [Fact]
        public async Task SubmitOrder_NonExistingUser_ReturnsBadRequestAndLogsWarning() 
        {
            //arrange
            var userId = Guid.NewGuid();
            var request = new CreateOrderDto
            {
                Items = new List<OrderItemDto>
                    {
                    new OrderItemDto
                    {
                        ProductId = Guid.NewGuid(),
                        Quantity = 2,
                        UnitPrice = 8
                    }
                }
            };
            _userRepositoryMock.Setup(ur => ur.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(default(User));
            //act

            var result = await _sut.SubmitOrderAsync(userId, request);

            //assert
            Assert.False(result.IsSuccessful);
            Assert.Equal(HttpStatusCode.BadRequest, result.StatusCode);
            Assert.Equal("Non existent user", result.ErrorMessage);

            _loggerMock.Verify(
                x => x.Log(
                LogLevel.Warning,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, _) => v.ToString().Contains("[OrderService][SubmitOrder] Non existent user attempted to submit order")),
                null,
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
                Times.Once);

            _auditServiceMock.Verify(
                s => s.AddNewLogAsync(It.IsAny<AuditLogAddDto>()),
                Times.Never);

        }
        [Fact]
        public async Task SubmitOrder_RepoThrowsException_ReturnsInternalServerErrorAndLogsError()
        {
            //arrange
            var userId = Guid.NewGuid();
            var user = new User
            {
                Id = userId
            };
            var request = new CreateOrderDto
            {
                Items = new List<OrderItemDto>
                    {
                    new OrderItemDto
                    {
                        ProductId = Guid.NewGuid(),
                        Quantity = 2,
                        UnitPrice = 8
                    }
                }
            };
            _userRepositoryMock.Setup(ur => ur.GetByIdAsync(It.IsAny<Guid>())).Throws(() => new Exception("Test exception"));
            //act

            var result = await _sut.SubmitOrderAsync(userId, request);

            //assert
            Assert.False(result.IsSuccessful);
            Assert.Equal(HttpStatusCode.InternalServerError, result.StatusCode);
            Assert.Equal("Failed to submit order", result.ErrorMessage);

            _loggerMock.Verify(
                x => x.Log(
                LogLevel.Error,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, _) => v.ToString().Contains("[OrderService][SubmitOrder] Unhandled Exception has occured")),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
                Times.Once);

            _auditServiceMock.Verify(
                s => s.AddNewLogAsync(It.IsAny<AuditLogAddDto>()),
                Times.Never);
        }
        [Fact]
        public async Task SubmitOrder_ValidFlow_ReturnsCreated()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var productId = Guid.NewGuid();
            var orderId = Guid.NewGuid();
            var orderItemId = Guid.NewGuid();

            var user = new User { Id = userId };

            var product = new Product
            {
                Id = productId,
                Name = "Test Product",
                StockQuantity = 10,
                Price = 5.0m
            };

            var orderItem = new OrderItem
            {
                Id = orderItemId,
                ProductId = productId,
                Product = product, // Important for ToDto()
                OrderedQuantity = 2,
                UnitPrice = product.Price
            };

            var order = new Order
            {
                Id = orderId,
                UserId = userId,
                OrderPrice = 10.0m,
                Status = OrderStatus.Submitted,
                Items = new List<OrderItem> { orderItem },
                OrderDate = DateTime.UtcNow
            };

            var request = new CreateOrderDto
            {
                Items = new List<OrderItemDto>
                    {
                    new OrderItemDto
                    {
                        ProductId = productId,
                        Quantity = 2,
                        UnitPrice = product.Price
                    }
                }
            };

            _userRepositoryMock.Setup(r => r.GetByIdAsync(userId)).ReturnsAsync(user);
            _productRepositoryMock.Setup(r => r.GetByIdAsync(productId)).ReturnsAsync(product);
            _orderRepositoryMock.Setup(r => r.CreateNewOrderAsync(userId)).ReturnsAsync(order);
            _orderRepositoryMock.Setup(r => r.AddOrderItemsAsync(It.IsAny<List<OrderItem>>()))
                .ReturnsAsync((List<OrderItem> items) =>
                {
                    foreach (var item in items)
                        item.Product = product;
                    return items;
                });
            _orderRepositoryMock.Setup(r => r.SaveChangesAsync()).Returns(Task.CompletedTask);

            // Act
            var result = await _sut.SubmitOrderAsync(userId, request);

            // Assert
            Assert.True(result.IsSuccessful);
            Assert.Equal(HttpStatusCode.Created, result.StatusCode);
            Assert.NotNull(result.Data);

            _auditServiceMock.Verify(
                s => s.AddNewLogAsync(It.IsAny<AuditLogAddDto>()),
                Times.Once);
        }

        [Fact]
        public async Task GetOrderById_NonExistingUser_ReturnsNotFoundAndLogsWarning()
        {
            //arrange
            var userId = Guid.NewGuid();
            var orderId = Guid.NewGuid();

            _userRepositoryMock.Setup(ur => ur.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(default(User));
            //act

            var result = await _sut.GetOrderByIdAsync(userId, orderId);

            //assert
            Assert.False(result.IsSuccessful);
            Assert.Equal(HttpStatusCode.NotFound, result.StatusCode);
            Assert.Equal("Non existent user", result.ErrorMessage);

            _loggerMock.Verify(
                x => x.Log(
                LogLevel.Warning,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, _) => v.ToString().Contains("[OrderService][GetOrderById] Non existent user attempted to fetch order")),
                null,
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
                Times.Once);

        }
        [Fact]
        public async Task GetOrderById_NonExistingOrder_ReturnsNotFoundAndLogsWarning() 
        {
            //arrange
            var userId = Guid.NewGuid();
            var orderId = Guid.NewGuid();

            var user = new User { Id = userId };

            _userRepositoryMock.Setup(ur => ur.GetByIdAsync(userId)).ReturnsAsync(user);
            _orderRepositoryMock.Setup(or=> or.GetByIdAsync(orderId)).ReturnsAsync(default(Order)); 
            //act

            var result = await _sut.GetOrderByIdAsync(userId, orderId);

            //assert
            Assert.False(result.IsSuccessful);
            Assert.Equal(HttpStatusCode.NotFound, result.StatusCode);
            Assert.Equal("Non existent order", result.ErrorMessage);

            _loggerMock.Verify(
                x => x.Log(
                LogLevel.Warning,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, _) => v.ToString().Contains("[OrderService][GetOrderById] Non existent order fetch attempt")),
                null,
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
                Times.Once);
        }
        [Fact]
        public async Task GetOrderById_RequestingUserAndOrderUserMismatch_ReturnsUnauthorizedAndLogsWarning() 
        {
            //arrange
            var userId = Guid.NewGuid();
            var orderId = Guid.NewGuid();

            var user = new User { Id = userId };
            var order = new Order { Id = orderId, UserId = Guid.NewGuid() };

            _userRepositoryMock.Setup(ur => ur.GetByIdAsync(userId)).ReturnsAsync(user);
            _orderRepositoryMock.Setup(or => or.GetByIdAsync(orderId)).ReturnsAsync(order);
            //act

            var result = await _sut.GetOrderByIdAsync(userId, orderId);

            //assert
            Assert.False(result.IsSuccessful);
            Assert.Equal(HttpStatusCode.Forbidden, result.StatusCode);

            _loggerMock.Verify(
                x => x.Log(
                LogLevel.Warning,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, _) => v.ToString().Contains("[OrderService][GetOrderById] User tried to fetch order beloning to different user")),
                null,
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
                Times.Once);
        }
        [Fact]
        public async Task GetOrderById_ValidFlow_ReturnsOk()
        {
            //arrange
            var userId = Guid.NewGuid();
            var orderId = Guid.NewGuid();

            var user = new User { Id = userId };
            var order = new Order { Id = orderId, UserId = user.Id };

            _userRepositoryMock.Setup(ur => ur.GetByIdAsync(userId)).ReturnsAsync(user);
            _orderRepositoryMock.Setup(or => or.GetByIdAsync(orderId)).ReturnsAsync(order);
            //act

            var result = await _sut.GetOrderByIdAsync(userId, orderId);

            //assert
            Assert.True(result.IsSuccessful);
            Assert.Equal(HttpStatusCode.OK, result.StatusCode);
            Assert.NotNull(result.Data);
        }

        [Fact]
        public async Task GetAllOrdersForUser_NonExistingUser_ReturnsNotFoundAndLogsWarning()
        {
            //arrange
            var userId = Guid.NewGuid();

            _userRepositoryMock.Setup(ur => ur.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(default(User));
            //act

            var result = await _sut.GetAllOrdersForUserAsync(userId);

            //assert
            Assert.False(result.IsSuccessful);
            Assert.Equal(HttpStatusCode.NotFound, result.StatusCode);
            Assert.Equal("Non existent user", result.ErrorMessage);

            _loggerMock.Verify(
                x => x.Log(
                LogLevel.Warning,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, _) => v.ToString().Contains("[OrderService][GetAllOrdersForUser] Non existent user attempted to fetch order history")),
                null,
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
                Times.Once);

        }
        [Fact]
        public async Task GetAllOrdersForUser_RepoThrowsException_ReturnsInternalServerErrorAndLogsError()
        {
            //arrange
            var userId = Guid.NewGuid();

            var user = new User { Id = userId };

            _userRepositoryMock.Setup(ur => ur.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(user);
            _orderRepositoryMock.Setup(or => or.GetAllForUserAsync(userId)).Throws(() => new DbUpdateException("Test exception"));
            //act

            var result = await _sut.GetAllOrdersForUserAsync(userId);

            //assert
            Assert.False(result.IsSuccessful);
            Assert.Equal(HttpStatusCode.InternalServerError, result.StatusCode);
            Assert.Equal("Failed to fetch the order history", result.ErrorMessage);

            _loggerMock.Verify(
                x => x.Log(
                LogLevel.Error,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, _) => v.ToString().Contains("[OrderService][GetAllOrdersForUser] Unhandled Exception has occured")),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
                Times.Once);

        }
        [Fact]
        public async Task GetAllOrdersForUser_ValidFlow_ReturnsOk()
        {
            //arrange
            var userId = Guid.NewGuid();

            var user = new User { Id = userId };

            var orderHistory = new List<Order> { 
                new Order { Id = Guid.NewGuid() },
                new Order { Id = Guid.NewGuid() }
            };

            _userRepositoryMock.Setup(ur => ur.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(user);
            _orderRepositoryMock.Setup(or => or.GetAllForUserAsync(userId)).ReturnsAsync(orderHistory);
            //act

            var result = await _sut.GetAllOrdersForUserAsync(userId);

            //assert
            Assert.True(result.IsSuccessful);
            Assert.Equal(HttpStatusCode.OK, result.StatusCode);
            Assert.NotNull(result.Data);

        }

        [Fact]
        public async Task CancelOrder_NonExistingUser_ReturnsNotFoundAndLogsWarning()
        {
            //arrange
            var userId = Guid.NewGuid();
            var orderId = Guid.NewGuid();

            _userRepositoryMock.Setup(ur => ur.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(default(User));
            //act

            var result = await _sut.CancelOrderAsync(userId, orderId);

            //assert
            Assert.False(result.IsSuccessful);
            Assert.Equal(HttpStatusCode.NotFound, result.StatusCode);
            Assert.Equal("Non existent user", result.ErrorMessage);

            _loggerMock.Verify(
                x => x.Log(
                LogLevel.Warning,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, _) => v.ToString().Contains("[OrderService][CancelOrder] Non existent user attempted to cancel order")),
                null,
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
                Times.Once);

            _auditServiceMock.Verify(
                s => s.AddNewLogAsync(It.IsAny<AuditLogAddDto>()),
                Times.Never);

        }
        [Fact]
        public async Task CancelOrder_NonExistingOrder_ReturnsNotFoundAndLogsWarning()
        {
            //arrange
            var userId = Guid.NewGuid();
            var orderId = Guid.NewGuid();

            var user = new User { Id = userId };

            _userRepositoryMock.Setup(ur => ur.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(user);
            _orderRepositoryMock.Setup(or=>or.GetByIdAsync(orderId)).ReturnsAsync(default(Order));
            //act

            var result = await _sut.CancelOrderAsync(userId, orderId);

            //assert
            Assert.False(result.IsSuccessful);
            Assert.Equal(HttpStatusCode.NotFound, result.StatusCode);
            Assert.Equal("Non existent order", result.ErrorMessage);

            _loggerMock.Verify(
                x => x.Log(
                LogLevel.Warning,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, _) => v.ToString().Contains("[OrderService][CancelOrder] Non existent order cancellation attempt")),
                null,
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
                Times.Once);

            _auditServiceMock.Verify(
                s => s.AddNewLogAsync(It.IsAny<AuditLogAddDto>()),
                Times.Never);

        }
        [Fact]
        public async Task CancelOrder_OrderBelonsToDifferentUser_ReturnsForbiddenAndLogsWarning()
        {
            //arrange
            var userId = Guid.NewGuid();
            var orderId = Guid.NewGuid();

            var user = new User { Id = userId };
            var order = new Order { Id = orderId, UserId = Guid.NewGuid() }; 

            _userRepositoryMock.Setup(ur => ur.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(user);
            _orderRepositoryMock.Setup(or => or.GetByIdAsync(orderId)).ReturnsAsync(order);
            //act

            var result = await _sut.CancelOrderAsync(userId, orderId);

            //assert
            Assert.False(result.IsSuccessful);
            Assert.Equal(HttpStatusCode.Forbidden, result.StatusCode);

            _loggerMock.Verify(
                x => x.Log(
                LogLevel.Warning,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, _) => v.ToString().Contains("[OrderService][CancelOrder] User tried to cancel order belonging to different user")),
                null,
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
                Times.Once);

            _auditServiceMock.Verify(
                s => s.AddNewLogAsync(It.IsAny<AuditLogAddDto>()),
                Times.Never);

        }
        [Fact]
        public async Task CancelOrder_OrderNotInSubmittedStatus_ReturnsBadRequestAndLogsWarning()
        {
            //arrange
            var userId = Guid.NewGuid();
            var orderId = Guid.NewGuid();

            var user = new User { Id = userId };
            var order = new Order { Id = orderId, UserId = userId, Status = OrderStatus.Completed };

            _userRepositoryMock.Setup(ur => ur.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(user);
            _orderRepositoryMock.Setup(or => or.GetByIdAsync(orderId)).ReturnsAsync(order);
            //act

            var result = await _sut.CancelOrderAsync(userId, orderId);

            //assert
            Assert.False(result.IsSuccessful);
            Assert.Equal(HttpStatusCode.BadRequest, result.StatusCode);
            Assert.Equal("Only orders in submitted state can be cancelled", result.ErrorMessage);

            _loggerMock.Verify(
                x => x.Log(
                LogLevel.Warning,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, _) => v.ToString().Contains("[OrderService][CancelOrder] User tried to cancel in other state than Submited")),
                null,
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
                Times.Once);

            _auditServiceMock.Verify(
                s => s.AddNewLogAsync(It.IsAny<AuditLogAddDto>()),
                Times.Never);

        }
        [Fact]
        public async Task CancelOrder_RepoThrowsAnException_ReturnsInternalServerErrorAndLogsError()
        {
            //arrange
            var userId = Guid.NewGuid();
            var orderId = Guid.NewGuid();

            var user = new User { Id = userId };
            var order = new Order { Id = orderId, UserId = userId, Status = OrderStatus.Completed };

            _userRepositoryMock.Setup(ur => ur.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(user);
            _orderRepositoryMock.Setup(or => or.GetByIdAsync(orderId)).Throws(() => new DbUpdateException("Test exception"));
            //act

            var result = await _sut.CancelOrderAsync(userId, orderId);

            //assert
            Assert.False(result.IsSuccessful);
            Assert.Equal(HttpStatusCode.InternalServerError, result.StatusCode);
            Assert.Equal("Failed to cancel the order", result.ErrorMessage);

            _loggerMock.Verify(
                x => x.Log(
                LogLevel.Error,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, _) => v.ToString().Contains("[OrderService][GetByIdAsync] Unhandled Exception has occured")),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
                Times.Once);

            _auditServiceMock.Verify(
                s => s.AddNewLogAsync(It.IsAny<AuditLogAddDto>()),
                Times.Never);

        }
        [Fact]
        public async Task CancelOrder_ValidInput_ReturnsOk()
        {
            //arrange
            var userId = Guid.NewGuid();
            var orderId = Guid.NewGuid();

            var user = new User { Id = userId };
            var order = new Order { Id = orderId, UserId = userId, Status = OrderStatus.Submitted };

            _userRepositoryMock.Setup(ur => ur.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(user);
            _orderRepositoryMock.Setup(or => or.GetByIdAsync(orderId)).ReturnsAsync(order);
            //act

            var result = await _sut.CancelOrderAsync(userId, orderId);

            //assert
            Assert.True(result.IsSuccessful);
            Assert.Equal(HttpStatusCode.OK, result.StatusCode);
            Assert.NotNull(result.Data);
            Assert.Equal(OrderStatus.Cancelled, result.Data.Status);

            _auditServiceMock.Verify(
                s => s.AddNewLogAsync(It.IsAny<AuditLogAddDto>()),
                Times.Once);

        }

    }
}
