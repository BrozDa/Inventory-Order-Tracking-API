using Inventory_Order_Tracking.API.Dtos;
using Inventory_Order_Tracking.API.Utils;

namespace InventoryManagement.API.Tests.Validators;

public class RegisterRequestValidatorTests
{
    public readonly RegisterRequestValidator _sut = new();

    [Fact]
    public void Validate_InvalidUsername_ReturnsFalse()
    {
        var result = _sut.Validate(new UserRegistrationDto
        { Username = "$pec!alChar", Password = "This1sV4lid!", Email = "test@test.com" }
        );
        Assert.False(result.IsValid);
    }

    [Fact]
    public void Validate_InvalidPassword_ReturnsFalse()
    {
        var result = _sut.Validate(new UserRegistrationDto
        { Username = "Username", Password = "weak", Email = "test@test.com" }
        );
        Assert.False(result.IsValid);
    }

    [Fact]
    public void Validate_InvalidEmail_ReturnsFalse()
    {
        var result = _sut.Validate(new UserRegistrationDto
        { Username = "Username", Password = "This1sV4lid!", Email = "testtest.com" }
        );
        Assert.False(result.IsValid);
    }

    [Fact]
    public void Validate_ValidRequest_ReturnsTrue()
    {
        var result = _sut.Validate(new UserRegistrationDto
        { Username = "Username", Password = "This1sV4lid!", Email = "test@test.com" }
        );
        Assert.True(result.IsValid);
    }
}