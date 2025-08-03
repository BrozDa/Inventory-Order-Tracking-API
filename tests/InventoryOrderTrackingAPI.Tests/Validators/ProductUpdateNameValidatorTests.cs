using Inventory_Order_Tracking.API.Dtos;
using Inventory_Order_Tracking.API.Validators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryManagement.API.Tests.Validators
{
    public class ProductUpdateNameValidatorTests
    {
        private readonly ProductUpdateNameValidator _sut;

        public ProductUpdateNameValidatorTests()
        {
            _sut = new();
        }
        [Fact]
        public void Validate_ValueNull_ReturnsFalse() 
        {
            //arrange
            var updatedName = new ProductUpdateNameDto
            {
                Name = null
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
            var updatedName = new ProductUpdateNameDto
            {
                Name = string.Empty,
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
            var updatedName = new ProductUpdateNameDto
            {
                Name = "This is name is too long, one would consider this very unnecessary",
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
            var updatedName = new ProductUpdateNameDto
            {
                Name = "$pec!4l Name#",
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
            var updatedName = new ProductUpdateNameDto
            {
                Name = "Top-Product Ultra 3000",
            };
            //act

            var result = _sut.Validate(updatedName);
            //assert

            Assert.True(result.IsValid);
        }

    }
}
