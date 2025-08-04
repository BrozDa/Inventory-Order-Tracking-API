using Castle.Core.Logging;
using Inventory_Order_Tracking.API.Controllers;
using Inventory_Order_Tracking.API.Domain;
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
    public class OrderControllerTests
    {
        public readonly OrderController _sut;
        public readonly Mock<IOrderService> _orderServiceMock = new();
        public readonly Mock<ICurrentUserService> _userServiceMock = new();

        public OrderControllerTests()
        {
            _sut = new OrderController(_userServiceMock.Object,_orderServiceMock.Object);
        }

        [Fact]
        public async Task PlaceOrder_InvalidUser_ReturnsUnauthorized()
        {
            Guid? userId = null;
            //arrange
            var request = new CreateOrderDto
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

            var request = new CreateOrderDto
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

            _orderServiceMock.Setup(s => s.SubmitOrder(It.IsAny<Guid>(), It.IsAny<CreateOrderDto>()))
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

            var request = new CreateOrderDto
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

            _orderServiceMock.Setup(s => s.SubmitOrder(It.IsAny<Guid>(), It.IsAny<CreateOrderDto>()))
                .ReturnsAsync(serviceResult);
            //act

            var result = await _sut.PlaceOrder(request);
            //assert
            Assert.IsType<CreatedAtActionResult>(result);
        }
    }
}
