using Inventory_Order_Tracking.API.Controllers;
using Inventory_Order_Tracking.API.Dtos;
using Inventory_Order_Tracking.API.Services.Interfaces;
using Inventory_Order_Tracking.API.Services.Shared;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

namespace InventoryManagement.API.Tests.Controllers
{
    public class ProductControllerTests
    {
        private readonly ProductsController _sut;
        private readonly Mock<IProductService> _serviceMock = new();
        private readonly Mock<ILogger<ProductsController>> _loggerMock = new();

        public ProductControllerTests()
        {
            _sut = new ProductsController(_serviceMock.Object, _loggerMock.Object);
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
        public async Task AdminsAdd_SuccessfulRequest_ReturnsCreated()
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
            Assert.IsType<CreatedResult>(result);
        }

        [Fact]
        public async Task AdminsAdd_FailedRequest_ReturnsOtherThanOk()
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

        //////////////////////////////////////////////////////////////////////////////////////////////////////////
        [Fact]
        public async Task AdminsUpdateName_FailedRequest_ReturnsOtherThanOk() 
        { 

            //arrange
            var id = Guid.NewGuid();
            var dto = new ProductUpdateNameDto { Name = "Changed Name" };

            var serviceResult = ProductServiceResult<ProductAdminDto>.NotFound();

            _serviceMock.Setup(s => s.UpdateNameAsync(It.IsAny<Guid>(), It.IsAny<string>())).ReturnsAsync(serviceResult);

            //act
            var result = await _sut.AdminsUpdateName(id, dto);

            //assert
            Assert.IsNotType<OkObjectResult>(result);

        }
        [Fact]
        public async Task AdminsUpdateName_SuccessfulRequest_ReturnsOk()
        {

            //arrange
            var id = Guid.NewGuid();
            var dto = new ProductUpdateNameDto { Name = "Changed Name" };

            var serviceResult = ProductServiceResult<ProductAdminDto>.Ok();

            _serviceMock.Setup(s => s.UpdateNameAsync(It.IsAny<Guid>(), It.IsAny<string>())).ReturnsAsync(serviceResult);

            //act
            var result = await _sut.AdminsUpdateName(id, dto);

            //assert
            Assert.IsType<OkObjectResult>(result);

        }
        [Fact]
        public async Task AdminsUpdateDescription_FailedRequest_ReturnsOtherThanOk()
        {

            //arrange
            var id = Guid.NewGuid();
            var dto = new ProductUpdateDescriptionDto { Description = "Changed Description" };

            var serviceResult = ProductServiceResult<ProductAdminDto>.NotFound();

            _serviceMock.Setup(s => s.UpdateDescriptionAsync(It.IsAny<Guid>(), It.IsAny<string>())).ReturnsAsync(serviceResult);

            //act
            var result = await _sut.AdminsUpdateDescription(id, dto);

            //assert
            Assert.IsNotType<OkObjectResult>(result);

        }
        [Fact]
        public async Task AdminsUpdateDescription_SuccessfulRequest_ReturnsOk()
        {

            //arrange
            var id = Guid.NewGuid();
            var dto = new ProductUpdateDescriptionDto { Description = "Changed Description" };

            var serviceResult = ProductServiceResult<ProductAdminDto>.Ok();

            _serviceMock.Setup(s => s.UpdateDescriptionAsync(It.IsAny<Guid>(), It.IsAny<string>())).ReturnsAsync(serviceResult);

            //act
            var result = await _sut.AdminsUpdateDescription(id, dto);

            //assert
            Assert.IsType<OkObjectResult>(result);

        }
        [Fact]
        public async Task AdminsUpdatePrice_FailedRequest_ReturnsOtherThanOk()
        {

            //arrange
            var id = Guid.NewGuid();
            var dto = new ProductUpdatePriceDto { Price= 13.24m};

            var serviceResult = ProductServiceResult<ProductAdminDto>.NotFound();

            _serviceMock.Setup(s => s.UpdatePriceAsync(It.IsAny<Guid>(), It.IsAny<decimal>())).ReturnsAsync(serviceResult);

            //act
            var result = await _sut.AdminsUpdatePrice(id, dto);

            //assert
            Assert.IsNotType<OkObjectResult>(result);

        }
        [Fact]
        public async Task AdminsUpdatePrice_SuccessfulRequest_ReturnsOk()
        {

            //arrange
            var id = Guid.NewGuid();
            var dto = new ProductUpdatePriceDto { Price = 13.24m };

            var serviceResult = ProductServiceResult<ProductAdminDto>.Ok();

            _serviceMock.Setup(s => s.UpdatePriceAsync(It.IsAny<Guid>(), It.IsAny<decimal>())).ReturnsAsync(serviceResult);

            //act
            var result = await _sut.AdminsUpdatePrice(id, dto);

            //assert
            Assert.IsType<OkObjectResult>(result);

        }
        [Fact]
        public async Task UpdateStock_FailedRequest_ReturnsOtherThanOk()
        {

            //arrange
            var id = Guid.NewGuid();
            var dto = new ProductUpdateStockDto { Stock = 13 };

            var serviceResult = ProductServiceResult<ProductAdminDto>.NotFound();

            _serviceMock.Setup(s => s.UpdateStockQuantityAsync(It.IsAny<Guid>(), It.IsAny<int>())).ReturnsAsync(serviceResult);

            //act
            var result = await _sut.AdminsUpdateStock(id, dto);

            //assert
            Assert.IsNotType<OkObjectResult>(result);

        }
        [Fact]
        public async Task UpdateStock_SuccessfulRequest_ReturnsOk()
        {

            //arrange
            var id = Guid.NewGuid();
            var dto = new ProductUpdateStockDto { Stock = 13 };

            var serviceResult = ProductServiceResult<ProductAdminDto>.Ok();

            _serviceMock.Setup(s => s.UpdateStockQuantityAsync(It.IsAny<Guid>(), It.IsAny<int>())).ReturnsAsync(serviceResult);

            //act
            var result = await _sut.AdminsUpdateStock(id, dto);

            //assert
            Assert.IsType<OkObjectResult>(result);

        }
        //////////////////////////////////////////////////////////////////////////////////////////////////////////
        [Fact]
        public async Task AdminsUpdate_SuccessfulRequest_ReturnsOk()
        {
            //arrange
            var id = Guid.NewGuid();
            var model = new ProductUpdateDto()
            {
                Name = "Updated Name",
                Description = "Updated Description",
                Price = 69m,
                StockQuantity = 15
            };

            var serviceResult = ProductServiceResult<ProductAdminDto>.Ok();

            _serviceMock.Setup(s => s.UpdateAsync(id, It.IsAny<ProductUpdateDto>())).ReturnsAsync(serviceResult);
            //act
            var result = await _sut.AdminsUpdate(id, model);

            //assert
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task AdminsUpdate_FailedRequest_ReturnsOtherThanOk()
        {
            //arrange
            var id = Guid.NewGuid();
            var model = new ProductUpdateDto()
            {
                Name = "Updated Name",
                Description = "Updated Description",
                Price = 69m,
                StockQuantity = 15
            };

            var serviceResult = ProductServiceResult<ProductAdminDto>.NotFound();

            _serviceMock.Setup(s => s.UpdateAsync(id, It.IsAny<ProductUpdateDto>())).ReturnsAsync(serviceResult);
            //act
            var result = await _sut.AdminsUpdate(id, model);

            //assert
            Assert.IsNotType<OkObjectResult>(result);
        }

        [Fact]
        public async Task AdminsDelete_FailedRequest_ReturnsNonOK()
        {
            //arrange
            var id = Guid.NewGuid();

            //act
            var serviceResult = ProductServiceResult<ProductAdminDto>.NotFound();
            _serviceMock.Setup(s => s.DeleteAsync(id)).ReturnsAsync(serviceResult);

            var result = await _sut.AdminsDelete(id);

            //assert
            Assert.IsNotType<OkObjectResult>(result);
        }

        [Fact]
        public async Task AdminsDelete_SuccessfulRequest_ReturnsNoContent()
        {
            //arrange
            var id = Guid.NewGuid();

            //act
            var serviceResult = ProductServiceResult<ProductAdminDto>.NoContent();
            _serviceMock.Setup(s => s.DeleteAsync(id)).ReturnsAsync(serviceResult);

            var result = await _sut.AdminsDelete(id);

            //assert
            Assert.IsType<NoContentResult>(result);
        }
    }
}