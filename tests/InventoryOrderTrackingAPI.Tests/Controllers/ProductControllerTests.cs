using Castle.Core.Logging;
using Inventory_Order_Tracking.API.Controllers;
using Inventory_Order_Tracking.API.Dtos;
using Inventory_Order_Tracking.API.Services.Interfaces;
using Inventory_Order_Tracking.API.Services.Shared;
using Inventory_Order_Tracking.API.Validators;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace InventoryManagement.API.Tests.Controllers
{
    public class ProductControllerTests
    {
        private readonly ProductsController _sut;
        private readonly Mock<IProductService> _serviceMock = new();
        private readonly StringValueValidator _validatorMock = new();
        private readonly Mock<ILogger<ProductsController>> _loggerMock = new();

        public ProductControllerTests()
        {
            _sut = new ProductsController(_serviceMock.Object, _validatorMock, _loggerMock.Object);
        }
        [Fact]
        public async Task CustomerGetAll_SuccessfulRequest_ReturnsOk()
        {

            var data = new List<ProductCustomerDto>();
            var serviceResult = ProductServiceResult<List<ProductCustomerDto>>.Ok(data);

            _serviceMock.Setup(s => s.CustomersGetAllAsync()).ReturnsAsync(serviceResult);

            var result = await _sut.CustomerGetAll();

            Assert.IsType<OkObjectResult>(result);

        }
        [Fact]
        public async Task CustomerGetAll_FailedRequest_ReturnsOtherThanOk()
        {

            var data = new List<ProductCustomerDto>();
            var serviceResult = ProductServiceResult<List<ProductCustomerDto>>.InternalServerError("Test result");

            _serviceMock.Setup(s => s.CustomersGetAllAsync()).ReturnsAsync(serviceResult);

            var result = await _sut.CustomerGetAll();

            Assert.IsNotType<OkObjectResult>(result);

        }
        [Fact]
        public async Task CustomerGetSingle_SuccessfulRequest_ReturnsOk()
        {
            var id = Guid.NewGuid();
            var data = new ProductCustomerDto() { Id = id };
            var serviceResult = ProductServiceResult<ProductCustomerDto>.Ok(data);


            _serviceMock.Setup(s => s.CustomersGetSingleAsync(id)).ReturnsAsync(serviceResult);

            var result = await _sut.CustomerGetSingle(id);

            Assert.IsType<OkObjectResult>(result);
        }
        [Fact]
        public async Task CustomerGetSingle_FailedRequest_ReturnsOk()
        {
            var id = Guid.NewGuid();
            var data = new ProductCustomerDto() { Id = id };
            var serviceResult = ProductServiceResult<ProductCustomerDto>.NotFound();


            _serviceMock.Setup(s => s.CustomersGetSingleAsync(id)).ReturnsAsync(serviceResult);

            var result = await _sut.CustomerGetSingle(id);

            Assert.IsNotType<OkObjectResult>(result);
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////
        [Fact]
        public async Task AdminsGetAll_SuccessfulRequest_ReturnsOk()
        {

            var data = new List<ProductAdminDto>();
            var serviceResult = ProductServiceResult<List<ProductAdminDto>>.Ok(data);

            _serviceMock.Setup(s => s.AdminsGetAllAsync()).ReturnsAsync(serviceResult);

            var result = await _sut.AdminsGetAll();

            Assert.IsType<OkObjectResult>(result);

        }
        [Fact]
        public async Task AdminsGetAll_FailedRequest_ReturnsOtherThanOk()
        {

            var data = new List<ProductAdminDto>();
            var serviceResult = ProductServiceResult<List<ProductAdminDto>>.InternalServerError("Test result");

            _serviceMock.Setup(s => s.AdminsGetAllAsync()).ReturnsAsync(serviceResult);

            var result = await _sut.AdminsGetAll();

            Assert.IsNotType<OkObjectResult>(result);

        }
        [Fact]
        public async Task AdminsGetSingle_SuccessfulRequest_ReturnsOk()
        {
            var id = Guid.NewGuid();
            var data = new ProductAdminDto() { Id = id };
            var serviceResult = ProductServiceResult<ProductAdminDto>.Ok(data);


            _serviceMock.Setup(s => s.AdminsGetSingleAsync(id)).ReturnsAsync(serviceResult);

            var result = await _sut.AdminsGetSingle(id);

            Assert.IsType<OkObjectResult>(result);
        }
        [Fact]
        public async Task AdminsGetSingle_FailedRequest_ReturnsOk()
        {
            var id = Guid.NewGuid();
            var data = new ProductAdminDto() { Id = id };
            var serviceResult = ProductServiceResult<ProductAdminDto>.NotFound();


            _serviceMock.Setup(s => s.AdminsGetSingleAsync(id)).ReturnsAsync(serviceResult);

            var result = await _sut.AdminsGetSingle(id);

            Assert.IsNotType<OkObjectResult>(result);
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////
        [Fact]
        public async Task AdminsAddStock_SuccessfulRequest_ReturnsOk()
        {
            //arrange
            var model = new ProductAddDto()
            {
                Name = "Valid name",
                Description = "Even mode Valid Description",
                Price = -69m, // INVALID PRICE
                StockQuantity = 2
            };

            var serviceResult = ProductServiceResult<ProductAdminDto>.Ok();

            _serviceMock.Setup(s => s.AddAsync(It.IsAny<ProductAddDto>())).ReturnsAsync(serviceResult);
            //act
            var result = await _sut.AdminsAdd(model);

            //assert
            Assert.IsType<OkObjectResult>(result);
        }
        [Fact]
        public async Task AdminsAddStock_FailedRequest_ReturnsOtherThanOk()
        {
            //arrange
            var model = new ProductAddDto()
            {
                Name = "Valid name",
                Description = "Even mode Valid Description",
                Price = 69m,
                StockQuantity = 2
            };

            var serviceResult = ProductServiceResult<ProductAdminDto>.NotFound();

            _serviceMock.Setup(s => s.AddAsync(It.IsAny<ProductAddDto>())).ReturnsAsync(serviceResult);
            //act
            var result = await _sut.AdminsAdd(model);

            //assert
            Assert.IsNotType<OkObjectResult>(result);
        }
    }
}
