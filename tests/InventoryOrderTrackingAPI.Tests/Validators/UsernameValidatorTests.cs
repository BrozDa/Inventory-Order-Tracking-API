using Inventory_Order_Tracking.API.Validators;

namespace InventoryManagement.API.Tests.Validators;

public class UsernameValidatorTests
{
    public readonly UsernameValidator _sut = new();

    [Fact]
    public void Validate_UsernameEmpty_ReturnsFalse()
    {
        var result = _sut.Validate("");
        Assert.False(result.IsValid);
    }

    [Fact]
    public void Validate_UsernameLessThanFiveChars_ReturnsFalse()
    {
        var result = _sut.Validate("test");
        Assert.False(result.IsValid);
    }

    [Fact]
    public void Validate_UsernameMoreThanSixTeenChars_ReturnsFalse()
    {
        var result = _sut.Validate("ThisStringIsMoreThanSixteenCharacterLong");
        Assert.False(result.IsValid);
    }

    [Fact]
    public void Validate_UsernameValidLengthContainsSpecialCharacters_ReturnsFalse()
    {
        var result = _sut.Validate("Test_W!th$Pec1alChars");
        Assert.False(result.IsValid);
    }

    [Fact]
    public void Validate_UsernameValidJustLetters_ReturnsTrue()
    {
        var result = _sut.Validate("ThisIsValid");
        Assert.True(result.IsValid);
    }

    [Fact]
    public void Validate_UsernameValidJustNumbers_ReturnsTrue()
    {
        var result = _sut.Validate("1234567");
        Assert.True(result.IsValid);
    }

    [Fact]
    public void Validate_UsernameValidLettersAndNumbers_ReturnsTrue()
    {
        var result = _sut.Validate("AlsoValid15");
        Assert.True(result.IsValid);
    }
}