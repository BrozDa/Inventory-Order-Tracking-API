using Inventory_Order_Tracking.API.Dtos;
using Inventory_Order_Tracking.API.Validators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryManagement.API.Tests.Validators
{
    public class UpdateProductValidatorTests
    {
        private readonly UpdateProductValidator _sut = new();
        [Fact]
        public async Task Validate_NameEmpty_ReturnsFalse()
        {
            //arrange
            var dto = new ProductUpdateDto()
            {
                Name = string.Empty,
                Description = "Valid description",
                Price = 1m,
                StockQuantity = 5
            };
            //act
            var result = _sut.Validate(dto);
            //assert
            Assert.False(result.IsValid);
        }
        [Fact]
        public async Task Validate_NameWithSpecialChars_ReturnsFalse()
        {
            //arrange
            var dto = new ProductUpdateDto()
            {
                Name = "$pecial Name!!!!",
                Description = "Valid description",
                Price = 1m,
                StockQuantity = 5
            };
            //act
            var result = _sut.Validate(dto);
            //assert
            Assert.False(result.IsValid);
        }
        [Fact]
        public async Task Validate_NameTooLong_ReturnsFalse()
        {
            //arrange
            var dto = new ProductUpdateDto()
            {
                Name = "This is too long name and I dont know what product would have name this long",
                Description = "Valid description",
                Price = 1m,
                StockQuantity = 5
            };
            //act
            var result = _sut.Validate(dto);
            //assert
            Assert.False(result.IsValid);
        }
        [Fact]
        public async Task Validate_DescriptionEmpty_ReturnsFalse()
        {
            //arrange
            var dto = new ProductUpdateDto()
            {
                Name = "Valid name",
                Description = string.Empty,
                Price = 1m,
                StockQuantity = 5
            };
            //act
            var result = _sut.Validate(dto);
            //assert
            Assert.False(result.IsValid);
        }
        [Fact]
        public async Task Validate_DescriptionWithSpecialChars_ReturnsFalse()
        {
            //arrange
            var dto = new ProductUpdateDto()
            {
                Name = "Valid name",
                Description = "$pec!al Description",
                Price = 1m,
                StockQuantity = 5
            };
            //act
            var result = _sut.Validate(dto);
            //assert
            Assert.False(result.IsValid);
        }
        [Fact]
        public async Task Validate_DescriptionTooLong_ReturnsFalse()
        {
            //arrange
            var dto = new ProductUpdateDto()
            {
                Name = "Valid name",
                Description = "This is too long description and I dont know what product would have description this long",
                Price = 1m,
                StockQuantity = 5
            };
            //act
            var result = _sut.Validate(dto);
            //assert
            Assert.False(result.IsValid);
        }
        [Fact]
        public async Task Validate_PriceNegative_ReturnsFalse()
        {
            //arrange
            var dto = new ProductUpdateDto()
            {
                Name = "Valid name",
                Description = "Valid description",
                Price = -110m,
                StockQuantity = 5
            };
            //act
            var result = _sut.Validate(dto);
            //assert
            Assert.False(result.IsValid);
        }
        [Fact]
        public async Task Validate_StockQuantityNegative_ReturnsFalse()
        {
            //arrange
            var dto = new ProductUpdateDto()
            {
                Name = "Valid name",
                Description = "Valid description",
                Price = 10m,
                StockQuantity = -5
            };
            //act
            var result = _sut.Validate(dto);
            //assert
            Assert.False(result.IsValid);
        }
        [Fact]
        public async Task Validate_ValidInputStockNull_ReturnsTrue()
        {
            //arrange
            var dto = new ProductUpdateDto()
            {
                Name = "Valid name",
                Description = "Valid description",
                Price = 10m,
                StockQuantity = null
            };
            //act
            var result = _sut.Validate(dto);
            //assert
            Assert.True(result.IsValid);
        }
        [Fact]
        public async Task Validate_ValidInputStockPositive_ReturnsTrue()
        {
            //arrange
            var dto = new ProductUpdateDto()
            {
                Name = "Valid name",
                Description = "Valid description",
                Price = 10m,
                StockQuantity = 5
            };
            //act
            var result = _sut.Validate(dto);
            //assert
            Assert.True(result.IsValid);
        }
        [Fact]
        public async Task Validate_AllValuesNull_ReturnsTrue()
        {
            //arrange
            var dto = new ProductUpdateDto()
            {
                Name = null,
                Description = null,
                Price = null,
                StockQuantity = null
            };
            //act
            var result = _sut.Validate(dto);
            //assert
            Assert.True(result.IsValid);
        }
        [Fact]
        public async Task Validate_ThreeValuesNull_ReturnsTrue()
        {
            //arrange
            var dto = new ProductUpdateDto()
            {
                Name = "Valid name",
                Description = null,
                Price = null,
                StockQuantity = null
            };
            //act
            var result = _sut.Validate(dto);
            //assert
            Assert.True(result.IsValid);
        }
        [Fact]
        public async Task Validate_TwoValuesNull_ReturnsTrue()
        {
            //arrange
            var dto = new ProductUpdateDto()
            {
                Name = null,
                Description = "Valid Description",
                Price = null,
                StockQuantity = 8
            };
            //act
            var result = _sut.Validate(dto);
            //assert
            Assert.True(result.IsValid);
        }
        [Fact]
        public async Task Validate_OneValueNull_ReturnsTrue()
        {
            //arrange
            var dto = new ProductUpdateDto()
            {
                Name = null,
                Description = "Valid Description",
                Price = 18m,
                StockQuantity = 8
            };
            //act
            var result = _sut.Validate(dto);
            //assert
            Assert.True(result.IsValid);
        }
    }
}

