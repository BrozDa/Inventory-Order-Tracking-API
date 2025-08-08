using Inventory_Order_Tracking.API.Configuration;
using Inventory_Order_Tracking.API.Validators;

namespace InventoryManagement.API.Tests.Validators
{
    public class EmailSettingsValidatorTests
    {
        private readonly EmailSettingsValidator _sut;

        public EmailSettingsValidatorTests()
        {
            _sut = new();
        }

        [Fact]
        public void Validate_SenderEmailNull_ReturnsFalse()
        {
            //arrange
            var settings = new EmailSettings
            {
                SenderEmail = null,
                Sender = "InventoryManagementAPI",
                Host = "localhost",
                Port = 25
            };

            //act
            var result = _sut.Validate(settings);

            //assert
            Assert.False(result.IsValid);
        }

        [Fact]
        public void Validate_SenderEmailEmpty_ReturnsFalse()
        {
            //arrange
            var settings = new EmailSettings
            {
                SenderEmail = string.Empty,
                Sender = "InventoryManagementAPI",
                Host = "localhost",
                Port = 25
            };

            //act
            var result = _sut.Validate(settings);

            //assert
            Assert.False(result.IsValid);
        }

        [Fact]
        public void Validate_SenderNull_ReturnsFalse()
        {
            //arrange
            var settings = new EmailSettings
            {
                SenderEmail = "test@test.com",
                Sender = null,
                Host = "localhost",
                Port = 25
            };

            //act
            var result = _sut.Validate(settings);

            //assert
            Assert.False(result.IsValid);
        }

        [Fact]
        public void Validate_SenderEmpty_ReturnsFalse()
        {
            //arrange
            var settings = new EmailSettings
            {
                SenderEmail = "test@test.com",
                Sender = string.Empty,
                Host = "localhost",
                Port = 25
            };

            //act
            var result = _sut.Validate(settings);

            //assert
            Assert.False(result.IsValid);
        }

        [Fact]
        public void Validate_HostNull_ReturnsFalse()
        {
            //arrange
            var settings = new EmailSettings
            {
                SenderEmail = "test@test.com",
                Sender = "InventoryManagementAPI",
                Host = null,
                Port = 25
            };

            //act
            var result = _sut.Validate(settings);

            //assert
            Assert.False(result.IsValid);
        }

        [Fact]
        public void Validate_HostEmpty_ReturnsFalse()
        {
            //arrange
            var settings = new EmailSettings
            {
                SenderEmail = "test@test.com",
                Sender = "InventoryManagementAPI",
                Host = string.Empty,
                Port = 25
            };

            //act
            var result = _sut.Validate(settings);

            //assert
            Assert.False(result.IsValid);
        }

        [Fact]
        public void Validate_PortEmpty_ReturnsFalse() //Default value of 0 will be applied
        {
            //arrange
            var settings = new EmailSettings
            {
                SenderEmail = "test@test.com",
                Sender = "InventoryManagementAPI",
                Host = "localhost"
            };

            //act
            var result = _sut.Validate(settings);

            //assert
            Assert.False(result.IsValid);
        }

        [Fact]
        public void Validate_PortNegative_ReturnsFalse()
        {
            //arrange
            var settings = new EmailSettings
            {
                SenderEmail = "test@test.com",
                Sender = "InventoryManagementAPI",
                Host = "localhost",
                Port = -63
            };

            //act
            var result = _sut.Validate(settings);

            //assert
            Assert.False(result.IsValid);
        }

        [Fact]
        public void Validate_ValidInput_ReturnsTrue()
        {
            //arrange
            var settings = new EmailSettings
            {
                SenderEmail = "test@test.com",
                Sender = "InventoryManagementAPI",
                Host = "localhost",
                Port = 25
            };

            //act
            var result = _sut.Validate(settings);

            //assert
            Assert.True(result.IsValid);
        }
    }
}