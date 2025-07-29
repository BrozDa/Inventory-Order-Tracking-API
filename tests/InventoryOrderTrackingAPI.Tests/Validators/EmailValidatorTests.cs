using Inventory_Order_Tracking.API.Validators;

namespace InventoryManagement.API.Tests.Validators;

public class EmailValidatorTests
{
    private readonly EmailValidator _sut = new();
    [Fact]
    public void Validate_EmptyEmail_ReturnsFalse()
    {
        var result = _sut.Validate("");
        Assert.False(result.IsValid);
    }
    [Fact]
    public void Validate_MissingAtSymbol_ReturnsFalse()
    {
        var result = _sut.Validate("TestTest.com");
        Assert.False(result.IsValid);
    }
    [Fact]
    public void Validate_NothingBeforeAtSymbol_ReturnsFalse()
    {
        var result = _sut.Validate("@Test.com");
        Assert.False(result.IsValid);
    }
    [Fact]
    public void Validate_NothingAfterAtSymbol_ReturnsFalse()
    {
        var result = _sut.Validate("Test@");
        Assert.False(result.IsValid);
    }
    [Fact]
    public void Validate_OnlyAtSymbol_ReturnsFalse()
    {
        var result = _sut.Validate("@");
        Assert.False(result.IsValid);
    }
    [Fact]
    public void Validate_MultipleAtSymbols_ReturnsFalse()
    {
        var result = _sut.Validate("Test@@Test.com");
        Assert.False(result.IsValid);
    }
    [Fact]
    public void Validate_ValidEmail_ReturnsTrue()
    {
        var result = _sut.Validate("Test@Test.com");
        Assert.True(result.IsValid);
    }
}
