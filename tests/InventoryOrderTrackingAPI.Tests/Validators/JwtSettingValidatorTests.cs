using Inventory_Order_Tracking.API.Configuration;
using Inventory_Order_Tracking.API.Validators;

namespace InventoryManagement.API.Tests.Validators
{
    public class JwtSettingValidatorTests
    {
        private readonly JwtSettingValidator _sut;

        public JwtSettingValidatorTests()
        {
            _sut = new JwtSettingValidator();
        }

        [Fact]
        public void Validate_ValueNull_ReturnsFalse()
        {
            var testSettings = new JwtSettings
            {
                Token = "TestToken",
                Audience = "ThisTest",
                Issuer = null,
                TokenExpirationDays = 13,
                RefreshTokenExpirationDays = 1
            };

            var result = _sut.Validate(testSettings);
            Assert.False(result.IsValid);
        }

        [Fact]
        public void Validate_ValueEmpty_ReturnsFalse()
        {
            var testSettings = new JwtSettings
            {
                Token = "TestToken",
                Audience = "",
                Issuer = "ThisTest",
                TokenExpirationDays = 13,
                RefreshTokenExpirationDays = 1
            };

            var result = _sut.Validate(testSettings);
            Assert.False(result.IsValid);
        }

        [Fact]
        public void Validate_ExpirationValueEqualToZero_ReturnsFalse()
        {
            var testSettings = new JwtSettings
            {
                Token = "TestToken",
                Audience = "ThisTest",
                Issuer = "ThisTest",
                TokenExpirationDays = 0,
                RefreshTokenExpirationDays = 1
            };

            var result = _sut.Validate(testSettings);
            Assert.False(result.IsValid);
        }

        [Fact]
        public void Validate_ExpirationValueLessThanZero_ReturnsFalse()
        {
            var testSettings = new JwtSettings
            {
                Token = "TestToken",
                Audience = "ThisTest",
                Issuer = "ThisTest",
                TokenExpirationDays = 13,
                RefreshTokenExpirationDays = -19
            };

            var result = _sut.Validate(testSettings);
            Assert.False(result.IsValid);
        }

        [Fact]
        public void Validate_Valid_ReturnsTrue()
        {
            var testSettings = new JwtSettings
            {
                Token = "TestToken",
                Audience = "ThisTest",
                Issuer = "ThisTest",
                TokenExpirationDays = 13,
                RefreshTokenExpirationDays = 7
            };

            var result = _sut.Validate(testSettings);
            Assert.True(result.IsValid);
        }
    }
}