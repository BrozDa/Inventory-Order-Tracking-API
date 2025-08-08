using Inventory_Order_Tracking.API.Controllers;
using Inventory_Order_Tracking.API.Domain;
using Inventory_Order_Tracking.API.Dtos;
using Inventory_Order_Tracking.API.Services.Interfaces;
using Inventory_Order_Tracking.API.Services.Shared;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace InventoryManagement.API.Tests.Controllers
{
    public class OrderControllerTests
    {
        public readonly OrderController _sut;
        public readonly Mock<IOrderService> _orderServiceMock = new();
        public readonly Mock<ICurrentUserService> _userServiceMock = new();

        public OrderControllerTests()
        {
            _sut = new OrderController(_userServiceMock.Object, _orderServiceMock.Object);
        }

        [Fact]
        public async Task PlaceOrder_InvalidUser_ReturnsUnauthorized()
        {
            Guid? userId = null;
            //arrange
            var request = new OrderCreateDto
            {
                Items = new List<OrderItemDto> {
                    new OrderItemDto
                    {
                        Id = Guid.NewGuid(),
                        ProductId = Guid.NewGuid(),
                        ProductName ="Test product 1",
                        UnitPrice = 1m,
                        Quantity = 3
                    },
                    new OrderItemDto
                    {
                        Id = Guid.NewGuid(),
                        ProductId = Guid.NewGuid(),
                        ProductName ="Test product 2",
                        UnitPrice = 19m,
                        Quantity = 34
                    }
                }
            };
            _userServiceMock.Setup(us => us.GetCurentUserId()).Returns(userId);

            //act
            var result = await _sut.PlaceOrder(request);

            //assert
            Assert.IsType<UnauthorizedObjectResult>(result);
        }

        [Fact]
        public async Task PlaceOrder_FailedRequest_ReturnsOtherThanOK()
        {
            Guid? userId = Guid.NewGuid();
            //arrange

            var request = new OrderCreateDto
            {
                Items = new List<OrderItemDto> {
                    new OrderItemDto
                    {
                        Id = Guid.NewGuid(),
                        ProductId = Guid.NewGuid(),
                        ProductName ="Test product 1",
                        UnitPrice = 1m,
                        Quantity = 3
                    },
                    new OrderItemDto
                    {
                        Id = Guid.NewGuid(),
                        ProductId = Guid.NewGuid(),
                        ProductName ="Test product 2",
                        UnitPrice = 19m,
                        Quantity = 34
                    }
                }
            };
            var serviceResult = ServiceResult<OrderDto>.NotFound("Non existent user");

            _userServiceMock.Setup(us => us.GetCurentUserId()).Returns(userId);

            _orderServiceMock.Setup(s => s.SubmitOrderAsync(It.IsAny<Guid>(), It.IsAny<OrderCreateDto>()))
                .ReturnsAsync(serviceResult);
            //act
            var result = await _sut.PlaceOrder(request);

            //assert
            Assert.IsNotType<CreatedResult>(result);
        }

        [Fact]
        public async Task PlaceOrder_SuccessfulRequest_ReturnsCreated()
        {
            //arrange
            Guid? userId = Guid.NewGuid();

            var request = new OrderCreateDto
            {
                Items = new List<OrderItemDto> {
                    new OrderItemDto
                    {
                        Id = Guid.NewGuid(),
                        ProductId = Guid.NewGuid(),
                        UnitPrice = 1m,
                        Quantity = 3
                    },
                    new OrderItemDto
                    {
                        Id = Guid.NewGuid(),
                        ProductId = Guid.NewGuid(),
                        UnitPrice = 19m,
                        Quantity = 34
                    }
                }
            };
            var orderDto = new OrderDto
            {
                Id = Guid.NewGuid(),
                OrderDate = DateTime.Now,
                OrderPrice = 154.6m,
                Status = OrderStatus.Submitted,
                UserId = userId.Value,
                Items = new List<OrderItemDto>
                {
                    new OrderItemDto
                    {
                        Id = Guid.NewGuid(),
                        ProductId = Guid.NewGuid(),
                        ProductName ="Test product 1",
                        UnitPrice = 1m,
                        Quantity = 3
                    },
                    new OrderItemDto
                    {
                        Id = Guid.NewGuid(),
                        ProductId = Guid.NewGuid(),
                        ProductName ="Test product 2",
                        UnitPrice = 19m,
                        Quantity = 34
                    }
                }
            };

            var serviceResult = ServiceResult<OrderDto>.Created(orderDto);

            _userServiceMock.Setup(us => us.GetCurentUserId()).Returns(userId);

            _orderServiceMock.Setup(s => s.SubmitOrderAsync(It.IsAny<Guid>(), It.IsAny<OrderCreateDto>()))
                .ReturnsAsync(serviceResult);
            //act

            var result = await _sut.PlaceOrder(request);
            //assert
            Assert.IsType<CreatedAtActionResult>(result);
        }

        [Fact]
        public async Task GetOrderById_InvalidUser_ReturnsUnauthorized()
        {
            //arrange
            Guid? userId = null;
            var orderId = Guid.NewGuid();

            _userServiceMock.Setup(us => us.GetCurentUserId()).Returns(userId);

            //act
            var result = await _sut.GetOrderById(orderId);

            //assert
            Assert.IsType<UnauthorizedObjectResult>(result);
        }

        [Fact]
        public async Task GetOrderById_FailedRequest_ReturnsOtherThanOK()
        {
            //arrange
            Guid? userId = Guid.NewGuid();
            var orderId = Guid.NewGuid();
            var order = new OrderDto { Id = orderId };

            var serviceResult = ServiceResult<OrderDto>.NotFound();

            _userServiceMock.Setup(us => us.GetCurentUserId()).Returns(userId);
            _orderServiceMock.Setup(os => os.GetOrderByIdAsync(userId.Value, orderId)).ReturnsAsync(serviceResult);
            //act
            var result = await _sut.GetOrderById(orderId);

            //assert
            Assert.IsNotType<OkObjectResult>(result);
        }

        [Fact]
        public async Task GetOrderById_SuccessfulRequest_ReturnsOk()
        {
            //arrange
            Guid? userId = Guid.NewGuid();
            var orderId = Guid.NewGuid();
            var orderDto = new OrderDto { Id = orderId };

            var serviceResult = ServiceResult<OrderDto>.Ok(orderDto);

            _userServiceMock.Setup(us => us.GetCurentUserId()).Returns(userId);
            _orderServiceMock.Setup(os => os.GetOrderByIdAsync(userId.Value, orderId)).ReturnsAsync(serviceResult);
            //act
            var result = await _sut.GetOrderById(orderId);

            //assert
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task GetOrderHistoryForUser_InvalidUser_ReturnsUnauthorized()
        {
            //arrange
            Guid? userId = null;

            _userServiceMock.Setup(us => us.GetCurentUserId()).Returns(userId);

            //act
            var result = await _sut.GetOrderHistoryForUser();

            //assert
            Assert.IsType<UnauthorizedObjectResult>(result);
        }

        [Fact]
        public async Task GetOrderHistoryForUser_FailedRequest_ReturnsOtherThanOk()
        {
            //arrange
            Guid? userId = Guid.NewGuid();
            var orderId = Guid.NewGuid();

            var serviceResult = ServiceResult<List<OrderDto>>.BadRequest("Test bad request");

            _userServiceMock.Setup(us => us.GetCurentUserId()).Returns(userId);

            _orderServiceMock.Setup(os => os.GetAllOrdersForUserAsync(userId.Value)).ReturnsAsync(serviceResult);
            //act
            var result = await _sut.GetOrderHistoryForUser();

            //assert
            Assert.IsNotType<OkObjectResult>(result);
        }

        [Fact]
        public async Task GetOrderHistoryForUser_SuccessfulRequest_ReturnsOk()
        {
            //arrange
            Guid? userId = Guid.NewGuid();
            var orderId = Guid.NewGuid();

            var orderHistory = new List<OrderDto> { new OrderDto { Id = orderId } };

            var serviceResult = ServiceResult<List<OrderDto>>.Ok(orderHistory);

            _userServiceMock.Setup(us => us.GetCurentUserId()).Returns(userId);

            _orderServiceMock.Setup(os => os.GetAllOrdersForUserAsync(userId.Value)).ReturnsAsync(serviceResult);

            //act
            var result = await _sut.GetOrderHistoryForUser();

            //assert
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task CancelOrder_InvalidUser_ReturnsUnauthorized()
        {
            //arrange
            Guid? userId = null;
            Guid orderId = Guid.NewGuid();

            _userServiceMock.Setup(us => us.GetCurentUserId()).Returns(userId);

            //act
            var result = await _sut.CancelOrder(orderId);

            //assert
            Assert.IsType<UnauthorizedObjectResult>(result);
        }

        [Fact]
        public async Task CancelOrder_FailedRequest_ReturnsOtherThanOk()
        {
            //arrange
            Guid? userId = Guid.NewGuid();
            Guid orderId = Guid.NewGuid();

            var serviceResult = ServiceResult<OrderDto>.BadRequest("Test bad request");

            _userServiceMock.Setup(us => us.GetCurentUserId()).Returns(userId);
            _orderServiceMock.Setup(os => os.CancelOrderAsync(userId.Value, orderId)).ReturnsAsync(serviceResult);
            //act
            var result = await _sut.CancelOrder(orderId);

            //assert
            Assert.IsNotType<OkObjectResult>(result);
        }

        [Fact]
        public async Task CancelOrder_SuccessfulRequest_ReturnsOk()
        {
            //arrange
            Guid? userId = Guid.NewGuid();
            var orderId = Guid.NewGuid();

            var order = new OrderDto { Id = orderId };

            var serviceResult = ServiceResult<OrderDto>.Ok(order);

            _userServiceMock.Setup(us => us.GetCurentUserId()).Returns(userId);

            _orderServiceMock.Setup(os => os.CancelOrderAsync(userId.Value, orderId)).ReturnsAsync(serviceResult);

            //act
            var result = await _sut.CancelOrder(orderId);

            //assert
            Assert.IsType<OkObjectResult>(result);
        }
    }
}