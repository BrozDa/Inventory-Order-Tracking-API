using Inventory_Order_Tracking.API.Dtos;
using Inventory_Order_Tracking.API.Validators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryManagement.API.Tests.Validators
{
    public class ProductUpdateDescriptionValidatorTests
    {
        private readonly ProductUpdateDescriptionValidator _sut;

        public ProductUpdateDescriptionValidatorTests()
        {
            _sut = new();
        }
        [Fact]
        public void Validate_ValueNull_ReturnsFalse()
        {
            //arrange
            var updatedName = new ProductUpdateDescriptionDto
            {
                Description = null
            };
            //act

            var result = _sut.Validate(updatedName);
            //assert

            Assert.False(result.IsValid);
        }
        [Fact]
        public void Validate_ValueEmpty_ReturnsFalse()
        {
            //arrange
            var updatedName = new ProductUpdateDescriptionDto
            {
                Description = string.Empty,
            };
            //act

            var result = _sut.Validate(updatedName);
            //assert

            Assert.False(result.IsValid);
        }
        [Fact]
        public void Validate_TooLong_ReturnsFalse()
        {
            //arrange
            var updatedName = new ProductUpdateDescriptionDto
            {
                Description = "This description is too long, one would consider this very unnecessary",
            };
            //act

            var result = _sut.Validate(updatedName);
            //assert

            Assert.False(result.IsValid);
        }
        [Fact]
        public void Validate_ValueWithSpecialChars_ReturnsFalse()
        {
            //arrange
            var updatedName = new ProductUpdateDescriptionDto
            {
                Description = "$pec!4l Description#",
            };
            //act

            var result = _sut.Validate(updatedName);
            //assert

            Assert.False(result.IsValid);
        }
        [Fact]
        public void Validate_ValidValue_ReturnsFalse()
        {
            //arrange
            var updatedName = new ProductUpdateDescriptionDto
            {
                Description = "Top-Product Ultra 3000",
            };
            //act

            var result = _sut.Validate(updatedName);
            //assert

            Assert.True(result.IsValid);
        }
    }
}
