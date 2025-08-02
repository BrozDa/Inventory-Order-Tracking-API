using Castle.Core.Logging;
using Inventory_Order_Tracking.API.Domain;
using Inventory_Order_Tracking.API.Dtos;
using Inventory_Order_Tracking.API.Models;
using Inventory_Order_Tracking.API.Repository.Interfaces;
using Inventory_Order_Tracking.API.Services;
using Microsoft.EntityFrameworkCore.Query.Internal;
using Microsoft.Extensions.Logging;
using Moq;
using System.Net;

namespace InventoryManagement.API.Tests.Services
{
    public class ProductServiceTests
    {
        private readonly ProductService _sut;

        private readonly Mock<IProductRepository> _productRepositoryMock = new();
        private readonly Mock<ILogger<ProductService>> _loggerMock = new();
        public ProductServiceTests()
        {
            _sut = new(_productRepositoryMock.Object, _loggerMock.Object);
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
            var product = new Product() { Id=productId, Name="Test product name" };
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

        
    }
}
