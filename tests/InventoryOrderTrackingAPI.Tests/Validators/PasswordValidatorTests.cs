using Inventory_Order_Tracking.API.Validators;

namespace InventoryOrderTracking.API.Tests.Validators;

public class PasswordValidatorTests
{
    private readonly PasswordValidator _sut = new();

    [Fact]
    public void Validate_PasswordEmpty_ReturnsFalse()
    {
        var result = _sut.Validate("");
        Assert.False(result.IsValid);
    }

    [Fact]
    public void Validate_PasswordUnderEightChars_ReturnsFalse()
    {
        var result = _sut.Validate("Test11");
        Assert.False(result.IsValid);
    }

    [Fact]
    public void Validate_PasswordWithoutUpperCase_ReturnsFalse()
    {
        var result = _sut.Validate("test1test");
        Assert.False(result.IsValid);
    }

    [Fact]
    public void Validate_PasswordWithoutlowerCase_ReturnsFalse()
    {
        var result = _sut.Validate("TEST1TEST");
        Assert.False(result.IsValid);
    }

    [Fact]
    public void Validate_PasswordWithoutNumberCase_ReturnsFalse()
    {
        var result = _sut.Validate("TestTestTest");
        Assert.False(result.IsValid);
    }

    [Fact]
    public void Validate_PasswordValid_ReturnsTrue()
    {
        var result = _sut.Validate("Th1sIsV4lid");
        Assert.True(result.IsValid);
    }

    [Fact]
    public void Validate_PasswordValidWithSpecialChars_ReturnsTrue()
    {
        var result = _sut.Validate("Th1s_Is#V4lid%%!");
        Assert.True(result.IsValid);
    }
}