using Inventory_Order_Tracking.API.Dtos;
using Inventory_Order_Tracking.API.Models;
using Inventory_Order_Tracking.API.Repository.Interfaces;
using Inventory_Order_Tracking.API.Services;
using Inventory_Order_Tracking.API.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using System.Net;

namespace InventoryManagement.API.Tests.Services
{
    public class ProductServiceTests
    {
        private readonly ProductService _sut;

        private readonly Mock<ICurrentUserService> _curentUserServiceMock = new();
        private readonly Mock<IAuditService> _auditServiceMock = new();
        private readonly Mock<IProductRepository> _productRepositoryMock = new();
        private readonly Mock<ILogger<ProductService>> _loggerMock = new();

        public ProductServiceTests()
        {
            _sut = new(
                _curentUserServiceMock.Object,
                _auditServiceMock.Object,
                _productRepositoryMock.Object,
                _loggerMock.Object);
        }

        [Fact]
        public async Task CustomersGetAllAsync_ExistingProducts_ReturnsOkWithAList()
        {
            //arrange
            var products = new List<Product>()
            {
                new Product
                {
                    Id = Guid.NewGuid(),
                    Name = "Test name",
                    Description = "Test description",
                    Price = 1.1m,
                    StockQuantity = 1,
                },
                new Product
                {
                    Id = Guid.NewGuid(),
                    Name = "Test name2",
                    Description = "Test description2",
                    Price = 1.1m,
                    StockQuantity = 5
                },
            };

            _productRepositoryMock.Setup(r => r.GetAllAsync()).ReturnsAsync(products);

            //act

            var result = await _sut.CustomersGetAllAsync();

            //assert
            Assert.True(result.IsSuccessful);
            Assert.NotEmpty(result.Data);
        }

        [Fact]
        public async Task CustomersGetAllAsync_NoExistingProducts_ReturnsOkWithEmptyList()
        {
            //arrange
            var products = new List<Product>();

            _productRepositoryMock.Setup(r => r.GetAllAsync()).ReturnsAsync(products);

            //act

            var result = await _sut.CustomersGetAllAsync();

            //assert
            Assert.True(result.IsSuccessful);
            Assert.Empty(result.Data);
        }

        [Fact]
        public async Task CustomersGetAllAsync_ThrowsAnException_ReturnsInternalServerErrorAndLogs()
        {
            //arrange

            _productRepositoryMock.Setup(r => r.GetAllAsync()).Throws(() => new ArgumentException("Test exception"));

            //act

            var result = await _sut.CustomersGetAllAsync();

            //assert
            Assert.False(result.IsSuccessful);
            Assert.Equal(HttpStatusCode.InternalServerError, result.StatusCode);
            Assert.Equal("Failed to fetch products from database", result.ErrorMessage);

            _loggerMock.Verify(
                x => x.Log(
                LogLevel.Error,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, _) => v.ToString().Contains("[CustomerGetAllAsync] Unhandled Exception has occured")),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
                Times.Once);
        }

        [Fact]
        public async Task CustomersGetSingleAsync_ExistingProduct_ReturnsOkWithProduct()
        {
            //arrange
            var productId = Guid.NewGuid();
            var product = new Product() { Id = productId, Name = "Test product name" };
            _productRepositoryMock.Setup(r => r.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(product);

            //act
            var result = await _sut.CustomersGetSingleAsync(productId);

            //assert
            Assert.True(result.IsSuccessful);
            Assert.Equal(HttpStatusCode.OK, result.StatusCode);
        }

        [Fact]
        public async Task CustomersGetSingleAsync_NoExistingProducts_ReturnsNotFound()
        {
            //arrange
            var productId = Guid.NewGuid();

            _productRepositoryMock.Setup(r => r.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(default(Product));

            //act
            var result = await _sut.CustomersGetSingleAsync(productId);

            //assert
            Assert.False(result.IsSuccessful);
            Assert.Equal(HttpStatusCode.NotFound, result.StatusCode);
        }

        [Fact]
        public async Task CustomersGetSingleAsync_ThrowsAnException_ReturnsInternalServerErrorAndLogs()
        {
            //arrange
            var productId = Guid.NewGuid();

            _productRepositoryMock.Setup(r => r.GetByIdAsync(productId)).Throws(() => new ArgumentException("Test exception"));

            //act

            var result = await _sut.CustomersGetSingleAsync(productId);

            //assert
            Assert.False(result.IsSuccessful);
            Assert.Equal(HttpStatusCode.InternalServerError, result.StatusCode);
            Assert.Equal("Failed to fetch product from database", result.ErrorMessage);

            _loggerMock.Verify(
                x => x.Log(
                LogLevel.Error,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, _) => v.ToString().Contains("[CustomerGetSingleAsync] Unhandled Exception has occured")),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
                Times.Once);
        }

        [Fact]
        public async Task AdminsGetAllAsync_ExistingProducts_ReturnsOkWithAList()
        {
            var products = new List<Product>()
            {
                new Product
                {
                    Id = Guid.NewGuid(),
                    Name = "Test name",
                    Description = "Test description",
                    Price = 1.1m,
                    StockQuantity = 1,
                },
                new Product
                {
                    Id = Guid.NewGuid(),
                    Name = "Test name2",
                    Description = "Test description2",
                    Price = 1.1m,
                    StockQuantity = 5
                },
            };

            _productRepositoryMock.Setup(r => r.GetAllAsync()).ReturnsAsync(products);

            //act

            var result = await _sut.AdminsGetAllAsync();

            //assert
            Assert.True(result.IsSuccessful);
            Assert.NotEmpty(result.Data);
        }

        [Fact]
        public async Task AdminsGetAllAsync_NoExistingProducts_ReturnsOkWithEmptyList()
        {
            //arrange
            var products = new List<Product>();

            _productRepositoryMock.Setup(r => r.GetAllAsync()).ReturnsAsync(products);

            //act

            var result = await _sut.AdminsGetAllAsync();

            //assert
            Assert.True(result.IsSuccessful);
            Assert.Empty(result.Data);
        }

        [Fact]
        public async Task AdminsGetAllAsync_ThrowsAnException_ReturnsInternalServerErrorAndLogs()
        {
            //arrange

            _productRepositoryMock.Setup(r => r.GetAllAsync()).Throws(() => new ArgumentException("Test exception"));

            //act

            var result = await _sut.AdminsGetAllAsync();

            //assert
            Assert.False(result.IsSuccessful);
            Assert.Equal(HttpStatusCode.InternalServerError, result.StatusCode);
            Assert.Equal("Failed to fetch products from database", result.ErrorMessage);

            _loggerMock.Verify(
                x => x.Log(
                LogLevel.Error,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, _) => v.ToString().Contains("[CustomerGetAllAsync] Unhandled Exception has occured")),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
                Times.Once);
        }

        [Fact]
        public async Task AdminsGetSingleAsync_ExistingProduct_ReturnsOkWithProduct()
        {
            //arrange
            var productId = Guid.NewGuid();
            var product = new Product() { Id = productId, Name = "Test product name" };
            _productRepositoryMock.Setup(r => r.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(product);

            //act
            var result = await _sut.AdminsGetSingleAsync(productId);

            //assert
            Assert.True(result.IsSuccessful);
            Assert.Equal(HttpStatusCode.OK, result.StatusCode);
        }

        [Fact]
        public async Task AdminsGetSingleAsync_NoExistingProducts_ReturnsNotFound()
        {
            //arrange
            var productId = Guid.NewGuid();

            _productRepositoryMock.Setup(r => r.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(default(Product));

            //act
            var result = await _sut.AdminsGetSingleAsync(productId);

            //assert
            Assert.False(result.IsSuccessful);
            Assert.Equal(HttpStatusCode.NotFound, result.StatusCode);
        }

        [Fact]
        public async Task AdminsGetSingleAsync_ThrowsAnException_ReturnsInternalServerErrorAndLogs()
        {
            //arrange
            var productId = Guid.NewGuid();

            _productRepositoryMock.Setup(r => r.GetByIdAsync(productId)).Throws(() => new ArgumentException("Test exception"));

            //act

            var result = await _sut.AdminsGetSingleAsync(productId);

            //assert
            Assert.False(result.IsSuccessful);
            Assert.Equal(HttpStatusCode.InternalServerError, result.StatusCode);
            Assert.Equal("Failed to fetch product from database", result.ErrorMessage);

            _loggerMock.Verify(
                x => x.Log(
                LogLevel.Error,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, _) => v.ToString().Contains("[CustomerGetSingleAsync] Unhandled Exception has occured")),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
                Times.Once);
        }

        [Fact]
        public async Task UpdateNameAsync_ExistingEntity_ReturnsOk()
        {
            //arrange
            var id = Guid.NewGuid();
            var existing = new Product
            {
                Id = id,
                Name = "Test name",
                Description = "Test description",
                Price = 12m,
                StockQuantity = 3
            };
            var newName = "Test name changed";

            _productRepositoryMock.Setup(r => r.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(existing);
            _curentUserServiceMock.Setup(s => s.GetCurentUserId()).Returns(Guid.NewGuid());

            //act
            var result = await _sut.UpdateNameAsync(id, newName);

            //assert
            Assert.True(result.IsSuccessful);
            Assert.Equal(HttpStatusCode.OK, result.StatusCode);
            Assert.Equal(newName, existing.Name);

            _auditServiceMock.Verify(
                s => s.AddNewLogAsync(It.IsAny<AuditLogAddDto>()),
                Times.Once);
        }

        [Fact]
        public async Task UpdateNameAsync_NonExistingEntity_ReturnsNotFoundAndLogs()
        {
            //arrange
            var id = Guid.NewGuid();
            var newName = "Test description changed";

            _productRepositoryMock.Setup(r => r.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(default(Product));
            _curentUserServiceMock.Setup(s => s.GetCurentUserId()).Returns(Guid.NewGuid());

            //act
            var result = await _sut.UpdateNameAsync(id, newName);

            //assert
            Assert.False(result.IsSuccessful);
            Assert.Equal(HttpStatusCode.NotFound, result.StatusCode);

            _loggerMock.Verify(
                x => x.Log(
                LogLevel.Warning,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, _) => v.ToString()
                    .Contains("[ProductService][UpdateNameAsync] Attempted name change for non-existing product")),
                    null,
                    It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
                Times.Once);

            _auditServiceMock.Verify(
                s => s.AddNewLogAsync(It.IsAny<AuditLogAddDto>()),
                Times.Never);
        }

        [Fact]
        public async Task UpdateNameAsync_ThrowsAnError_ReturnsInternalServerErrorAndLogs()
        {
            //arrange
            var id = Guid.NewGuid();
            var newName = "Test name changed";

            _productRepositoryMock.Setup(r => r.GetByIdAsync(It.IsAny<Guid>())).Throws(() => new DbUpdateException("Test exception"));
            _curentUserServiceMock.Setup(s => s.GetCurentUserId()).Returns(Guid.NewGuid());

            //act
            var result = await _sut.UpdateNameAsync(id, newName);

            //assert
            Assert.False(result.IsSuccessful);
            Assert.Equal(HttpStatusCode.InternalServerError, result.StatusCode);
            Assert.Equal("Failed to update name", result.ErrorMessage);

            _loggerMock.Verify(
                x => x.Log(
                LogLevel.Error,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, _) => v.ToString()
                    .Contains("[UpdateNameAsync] Unhandled Exception has occured")),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
                Times.Once);

            _auditServiceMock.Verify(
                s => s.AddNewLogAsync(It.IsAny<AuditLogAddDto>()),
                Times.Never);
        }

        [Fact]
        public async Task UpdateDescriptionAsync_ExistingEntity_ReturnsOk()
        {
            //arrange
            var id = Guid.NewGuid();
            var existing = new Product
            {
                Id = id,
                Name = "Test name",
                Description = "Test description",
                Price = 12m,
                StockQuantity = 3
            };
            var newDescription = "Test description changed";

            _productRepositoryMock.Setup(r => r.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(existing);
            _curentUserServiceMock.Setup(s => s.GetCurentUserId()).Returns(Guid.NewGuid());

            //act
            var result = await _sut.UpdateDescriptionAsync(id, newDescription);

            //assert
            Assert.True(result.IsSuccessful);
            Assert.Equal(HttpStatusCode.OK, result.StatusCode);
            Assert.Equal(newDescription, existing.Description);

            _auditServiceMock.Verify(
                s => s.AddNewLogAsync(It.IsAny<AuditLogAddDto>()),
                Times.Once);
        }

        [Fact]
        public async Task UpdateDescriptionAsync_NonExistingEntity_ReturnsNotFoundAndLogs()
        {
            //arrange
            var id = Guid.NewGuid();
            var newDescription = "Test description changed";

            _productRepositoryMock.Setup(r => r.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(default(Product));
            _curentUserServiceMock.Setup(s => s.GetCurentUserId()).Returns(Guid.NewGuid());

            //act
            var result = await _sut.UpdateDescriptionAsync(id, newDescription);

            //assert
            Assert.False(result.IsSuccessful);
            Assert.Equal(HttpStatusCode.NotFound, result.StatusCode);

            _loggerMock.Verify(
                x => x.Log(
                LogLevel.Warning,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, _) => v.ToString()
                    .Contains("[ProductService][UpdateDescriptionAsync] Attempted description change for non-existing product")),
                    null,
                    It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
                Times.Once);

            _auditServiceMock.Verify(
                s => s.AddNewLogAsync(It.IsAny<AuditLogAddDto>()),
                Times.Never);
        }

        [Fact]
        public async Task UpdateDescriptionAsync_ThrowsAnError_ReturnsInternalServerErrorAndLogs()
        {
            //arrange
            var id = Guid.NewGuid();
            var newDescription = "Test description changed";

            _productRepositoryMock.Setup(r => r.GetByIdAsync(It.IsAny<Guid>())).Throws(() => new DbUpdateException("Test exception"));
            _curentUserServiceMock.Setup(s => s.GetCurentUserId()).Returns(Guid.NewGuid());

            //act
            var result = await _sut.UpdateDescriptionAsync(id, newDescription);

            //assert
            Assert.False(result.IsSuccessful);
            Assert.Equal(HttpStatusCode.InternalServerError, result.StatusCode);
            Assert.Equal("Failed to update description", result.ErrorMessage);

            _loggerMock.Verify(
                x => x.Log(
                LogLevel.Error,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, _) => v.ToString()
                    .Contains("[UpdateDescriptionAsync] Unhandled Exception has occured")),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
                Times.Once);

            _auditServiceMock.Verify(
                s => s.AddNewLogAsync(It.IsAny<AuditLogAddDto>()),
                Times.Never);
        }

        [Fact]
        public async Task UpdatePriceAsync_ExistingEntity_ReturnsOk()
        {
            //arrange
            var id = Guid.NewGuid();
            var existing = new Product
            {
                Id = id,
                Name = "Test name",
                Description = "Test description",
                Price = 12m,
                StockQuantity = 3
            };
            var newPrice = 17m;

            _productRepositoryMock.Setup(r => r.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(existing);
            _curentUserServiceMock.Setup(s => s.GetCurentUserId()).Returns(Guid.NewGuid());

            //act
            var result = await _sut.UpdatePriceAsync(id, newPrice);

            //assert
            Assert.True(result.IsSuccessful);
            Assert.Equal(HttpStatusCode.OK, result.StatusCode);
            Assert.Equal(newPrice, existing.Price);

            _auditServiceMock.Verify(
                s => s.AddNewLogAsync(It.IsAny<AuditLogAddDto>()),
                Times.Once);
        }

        [Fact]
        public async Task UpdatePriceAsync_NonExistingEntity_ReturnsNotFoundAndLogs()
        {
            //arrange
            var id = Guid.NewGuid();
            var newPrice = 12m;

            _productRepositoryMock.Setup(r => r.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(default(Product));
            _curentUserServiceMock.Setup(s => s.GetCurentUserId()).Returns(Guid.NewGuid());

            //act
            var result = await _sut.UpdatePriceAsync(id, newPrice);

            //assert
            Assert.False(result.IsSuccessful);
            Assert.Equal(HttpStatusCode.NotFound, result.StatusCode);

            _loggerMock.Verify(
                x => x.Log(
                LogLevel.Warning,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, _) => v.ToString()
                    .Contains("[ProductService][UpdatePriceAsync] Attempted price change for non-existing product")),
                    null,
                    It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
                Times.Once);

            _auditServiceMock.Verify(
                s => s.AddNewLogAsync(It.IsAny<AuditLogAddDto>()),
                Times.Never);
        }

        [Fact]
        public async Task UpdatePriceAsync_ThrowsAnError_ReturnsInternalServerErrorAndLogs()
        {
            //arrange
            var id = Guid.NewGuid();
            var newPrice = 12.12m;

            _productRepositoryMock.Setup(r => r.GetByIdAsync(It.IsAny<Guid>())).Throws(() => new DbUpdateException("Test exception"));
            _curentUserServiceMock.Setup(s => s.GetCurentUserId()).Returns(Guid.NewGuid());

            //act
            var result = await _sut.UpdatePriceAsync(id, newPrice);

            //assert
            Assert.False(result.IsSuccessful);
            Assert.Equal(HttpStatusCode.InternalServerError, result.StatusCode);
            Assert.Equal("Failed to update price", result.ErrorMessage);

            _loggerMock.Verify(
                x => x.Log(
                LogLevel.Error,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, _) => v.ToString()
                    .Contains("[UpdatePriceAsync] Unhandled Exception has occured")),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
                Times.Once);

            _auditServiceMock.Verify(
                s => s.AddNewLogAsync(It.IsAny<AuditLogAddDto>()),
                Times.Never);
        }

        [Fact]
        public async Task UpdateStockQuantityAsync_ExistingEntity_ReturnsOk()
        {
            //arrange
            var id = Guid.NewGuid();
            var existing = new Product
            {
                Id = id,
                Name = "Test name",
                Description = "Test description",
                Price = 12m,
                StockQuantity = 3
            };
            var newStockSquantity = 15;

            _productRepositoryMock.Setup(r => r.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(existing);
            _curentUserServiceMock.Setup(s => s.GetCurentUserId()).Returns(Guid.NewGuid());

            //act
            var result = await _sut.UpdateStockQuantityAsync(id, newStockSquantity);

            //assert
            Assert.True(result.IsSuccessful);
            Assert.Equal(HttpStatusCode.OK, result.StatusCode);
            Assert.Equal(newStockSquantity, existing.StockQuantity);

            _auditServiceMock.Verify(
                s => s.AddNewLogAsync(It.IsAny<AuditLogAddDto>()),
                Times.Once);
        }

        [Fact]
        public async Task UpdateStockQuantityAsync_NonExistingEntity_ReturnsNotFoundAndLogs()
        {
            //arrange
            var id = Guid.NewGuid();
            var newQuantity = 18;

            _productRepositoryMock.Setup(r => r.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(default(Product));
            _curentUserServiceMock.Setup(s => s.GetCurentUserId()).Returns(Guid.NewGuid());

            //act
            var result = await _sut.UpdateStockQuantityAsync(id, newQuantity);

            //assert
            Assert.False(result.IsSuccessful);
            Assert.Equal(HttpStatusCode.NotFound, result.StatusCode);

            _loggerMock.Verify(
                x => x.Log(
                LogLevel.Warning,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, _) => v.ToString()
                    .Contains("[ProductService][UpdateStockQuantityAsync] Attempted stock change for non-existing product")),
                    null,
                    It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
                Times.Once);

            _auditServiceMock.Verify(
                s => s.AddNewLogAsync(It.IsAny<AuditLogAddDto>()),
                Times.Never);
        }

        [Fact]
        public async Task UpdateStockQuantityAsync_ThrowsAnError_ReturnsInternalServerErrorAndLogs()
        {
            //arrange
            var id = Guid.NewGuid();
            var newStockQuantity = 12;

            _productRepositoryMock.Setup(r => r.GetByIdAsync(It.IsAny<Guid>())).Throws(() => new DbUpdateException("Test exception"));
            _curentUserServiceMock.Setup(s => s.GetCurentUserId()).Returns(Guid.NewGuid());

            //act
            var result = await _sut.UpdateStockQuantityAsync(id, newStockQuantity);

            //assert
            Assert.False(result.IsSuccessful);
            Assert.Equal(HttpStatusCode.InternalServerError, result.StatusCode);
            Assert.Equal("Failed to update stock quantity", result.ErrorMessage);

            _loggerMock.Verify(
                x => x.Log(
                LogLevel.Error,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, _) => v.ToString()
                    .Contains("[UpdateStockQuantityAsync] Unhandled Exception has occured")),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
                Times.Once);

            _auditServiceMock.Verify(
                s => s.AddNewLogAsync(It.IsAny<AuditLogAddDto>()),
                Times.Never);
        }

        [Fact]
        public async Task AddAsync_RepoThrowsException_ReturnsInternalServerErrorAndLogsError()
        {
            //arrange

            var dto = new ProductAddDto()
            {
                Name = "Valid name",
                Description = "Even mode Valid Description",
                Price = 69m,
                StockQuantity = 2
            };

            _productRepositoryMock.Setup(r => r.AddAsync(It.IsAny<Product>())).Throws(() => new DbUpdateException("Test exception"));
            _curentUserServiceMock.Setup(s => s.GetCurentUserId()).Returns(Guid.NewGuid());

            //act
            var result = await _sut.AddAsync(dto);

            //assert
            Assert.False(result.IsSuccessful);
            Assert.Equal(HttpStatusCode.InternalServerError, result.StatusCode);
            Assert.Equal("Failed to create new product", result.ErrorMessage);

            _loggerMock.Verify(
                x => x.Log(
                LogLevel.Error,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, _) => v.ToString()
                    .Contains("[AddAsync] Unhandled Exception has occured")),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
                Times.Once);

            _auditServiceMock.Verify(
                s => s.AddNewLogAsync(It.IsAny<AuditLogAddDto>()),
                Times.Never);
        }

        [Fact]
        public async Task AddAsync_SuccessfulFlow_ReturnsCreated()
        {
            //arrange
            var dto = new ProductAddDto()
            {
                Name = "Valid name",
                Description = "Even mode Valid Description",
                Price = 69m,
                StockQuantity = 2
            };
            var addedProduct = new Product()
            {
                Id = Guid.NewGuid(),
                Name = "Valid name",
                Description = "Even mode Valid Description",
                Price = 69m,
                StockQuantity = 2
            };

            _productRepositoryMock.Setup(r => r.AddAsync(It.IsAny<Product>())).ReturnsAsync(addedProduct);
            _curentUserServiceMock.Setup(s => s.GetCurentUserId()).Returns(Guid.NewGuid()); 
            //act
            var result = await _sut.AddAsync(dto);

            //assert
            Assert.True(result.IsSuccessful);
            Assert.Equal(HttpStatusCode.Created, result.StatusCode);

            _auditServiceMock.Verify(
                s => s.AddNewLogAsync(It.IsAny<AuditLogAddDto>()),
                Times.Once);
        }

        [Fact]
        public async Task UpdateAsync_NonExistingId_ReturnsNotFoundAndLogsWarning()
        {
            //arrange
            var id = Guid.NewGuid();
            var dto = new ProductUpdateDto
            {
                Name = "UpdatedName",
                Description = null,
                Price = 15m,
                StockQuantity = 8
            };

            _productRepositoryMock.Setup(r => r.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(default(Product));
            _curentUserServiceMock.Setup(s => s.GetCurentUserId()).Returns(Guid.NewGuid());

            //act
            var result = await _sut.UpdateAsync(id, dto);

            //assert
            Assert.False(result.IsSuccessful);
            Assert.Equal(HttpStatusCode.NotFound, result.StatusCode);

            _loggerMock.Verify(
                x => x.Log(
                LogLevel.Warning,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, _) => v.ToString()
                    .Contains("[ProductService][UpdateNameAsync] Attempted name change for non-existing product")),
                    null,
                    It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
                Times.Once);

            _auditServiceMock.Verify(
                s => s.AddNewLogAsync(It.IsAny<AuditLogAddDto>()),
                Times.Never);
        }

        [Fact]
        public async Task UpdateAsync_RepoThrowsException_ReturnsInternalServerErrorAndLogsError()
        {
            //arrange
            var id = Guid.NewGuid();
            var dto = new ProductUpdateDto
            {
                Name = "UpdatedName",
                Description = null,
                Price = 15m,
                StockQuantity = 8
            };

            _productRepositoryMock.Setup(r => r.GetByIdAsync(It.IsAny<Guid>()))
                .Throws(() => new DbUpdateException("Test exception"));
            _curentUserServiceMock.Setup(s => s.GetCurentUserId()).Returns(Guid.NewGuid());

            //act
            var result = await _sut.UpdateAsync(id, dto);

            //assert
            Assert.False(result.IsSuccessful);
            Assert.Equal(HttpStatusCode.InternalServerError, result.StatusCode);
            Assert.Equal("Failed to update product", result.ErrorMessage);

            _loggerMock.Verify(
                x => x.Log(
                LogLevel.Error,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, _) => v.ToString()
                    .Contains("[UpdateAsync] Unhandled Exception has occured")),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
                Times.Once);

            _auditServiceMock.Verify(
                s => s.AddNewLogAsync(It.IsAny<AuditLogAddDto>()),
                Times.Never);
        }

        [Fact]
        public async Task UpdateAsync_ValidEntity_ReturnsOk()
        {
            //arrange
            var id = Guid.NewGuid();
            var requestDto = new ProductUpdateDto
            {
                Name = "UpdatedName",
                Description = null,
                Price = 15m,
                StockQuantity = 8
            };

            var entity = new Product
            {
                Id = id,
                Name = "UpdatedName",
                Description = null,
                Price = 15m,
                StockQuantity = 8
            };

            _productRepositoryMock.Setup(r => r.GetByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync(entity);
            _curentUserServiceMock.Setup(s => s.GetCurentUserId()).Returns(Guid.NewGuid());

            //act
            var result = await _sut.UpdateAsync(id, requestDto);

            //assert
            Assert.True(result.IsSuccessful);
            Assert.Equal(HttpStatusCode.OK, result.StatusCode);

            _auditServiceMock.Verify(
                s => s.AddNewLogAsync(It.IsAny<AuditLogAddDto>()),
                Times.Once);
        }

        [Fact]
        public async Task DeleteAsync_NonExistingId_ReturnsNotFoundAndLogsWarning()
        {
            //arrange
            var id = Guid.NewGuid();

            _productRepositoryMock.Setup(r => r.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(default(Product));
            _curentUserServiceMock.Setup(s => s.GetCurentUserId()).Returns(Guid.NewGuid());

            //act
            var result = await _sut.DeleteAsync(id);
            //assert

            Assert.False(result.IsSuccessful);
            Assert.Equal(HttpStatusCode.NotFound, result.StatusCode);

            _loggerMock.Verify(
                x => x.Log(
                LogLevel.Warning,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, _) => v.ToString()
                    .Contains("[ProductService][DeleteAsync] Attempted deletion of non-existing product")),
                    null,
                    It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
                Times.Once);

            _auditServiceMock.Verify(
                s => s.AddNewLogAsync(It.IsAny<AuditLogAddDto>()),
                Times.Never);
        }

        [Fact]
        public async Task DeleteAsync_RepoThrowsException_ReturnsInternalServerErrorAndLogsWarning()
        {
            //arrange
            var id = Guid.NewGuid();

            _productRepositoryMock.Setup(r => r.GetByIdAsync(It.IsAny<Guid>()))
                .Throws(() => new DbUpdateException("Test exception"));
            _curentUserServiceMock.Setup(s => s.GetCurentUserId()).Returns(Guid.NewGuid());

            //act
            var result = await _sut.DeleteAsync(id);
            //assert

            Assert.False(result.IsSuccessful);
            Assert.Equal(HttpStatusCode.InternalServerError, result.StatusCode);

            _loggerMock.Verify(
                x => x.Log(
                LogLevel.Error,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, _) => v.ToString()
                    .Contains("[DeleteAsync] Unhandled Exception has occured")),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
                Times.Once);

            _auditServiceMock.Verify(
                s => s.AddNewLogAsync(It.IsAny<AuditLogAddDto>()),
                Times.Never);
        }

        [Fact]
        public async Task DeleteAsync_ValidFlow_ReturnsNoContent()
        {
            //arrange
            var id = Guid.NewGuid();

            var entity = new Product()
            {
                Id = Guid.NewGuid(),
                Name = "Valid name",
                Description = "Even more valid Description",
                Price = 69m,
                StockQuantity = 2
            };

            _productRepositoryMock.Setup(r => r.GetByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync(entity);
            _curentUserServiceMock.Setup(s => s.GetCurentUserId()).Returns(Guid.NewGuid());

            //act
            var result = await _sut.DeleteAsync(id);
            //assert

            Assert.True(result.IsSuccessful);
            Assert.Equal(HttpStatusCode.NoContent, result.StatusCode);

            _auditServiceMock.Verify(
                s => s.AddNewLogAsync(It.IsAny<AuditLogAddDto>()),
                Times.Once);
        }
    }
}