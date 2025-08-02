using Inventory_Order_Tracking.API.Domain;
using Inventory_Order_Tracking.API.Validators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryManagement.API.Tests.Validators
{
    public class StringValueValidatorTests
    {
        StringValueValidator _sut;

        public StringValueValidatorTests()
        {
            _sut = new StringValueValidator();  
        }
        [Fact]
        public void Validate_NullValue_ReturnsFalse() 
        {
            //arrange
            string? data = null;
            //act
            var validationResult = _sut.Validate(new StringWrapper { Value = data});
            //assert
            Assert.False(validationResult.IsValid);
        }
        [Fact]
        public void Validate_EmptyValue_ReturnsFalse() 
        {
            //arrange
            string? data = "";
            //act
            var validationResult = _sut.Validate(new StringWrapper { Value = data });
            //assert
            Assert.False(validationResult.IsValid);
        }
        [Fact]
        public void Validate_ValueContainingSpecialChar_ReturnsFalse() 
        {
            //arrange
            string? data = "NameW!th$pec1alChar";
            //act
            var validationResult = _sut.Validate(new StringWrapper { Value = data });
            //assert
            Assert.False(validationResult.IsValid);
        }
        [Fact]
        public void Validate_ValueTooLong_ReturnsFalse() 
        {
            //arrange
            string? data = "This Name Is Valid Just Too long for it to be actually valid new product name";
            //act
            var validationResult = _sut.Validate(new StringWrapper { Value = data });
            //assert
            Assert.False(validationResult.IsValid);
        }
        [Fact]
        public void Validate_ValidName_ReturnsTrue() 
        {
            //arrange
            string? data = "Bread";
            //act
            var validationResult = _sut.Validate(new StringWrapper { Value = data });
            //assert
            Assert.True(validationResult.IsValid);
        }

    }
}
