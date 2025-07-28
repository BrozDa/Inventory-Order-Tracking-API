using Inventory_Order_Tracking.API.Services;

namespace InventoryOrderTracking.API.Tests.Services;

public class PasswordServiceTests
{
    private readonly PasswordService _sut = new();
    private readonly string testPw = "test";
    [Fact]
    public void GenerateHashAndSalt_ReturnsNonEmptyValues()
    {
        var (hash, salt) = _sut.GenerateHashAndSalt(testPw);

        Assert.False(string.IsNullOrEmpty(hash));
        Assert.False(string.IsNullOrEmpty(salt));
    }
    [Fact]
    public void GenerateHashAndSalt_SamePW_DifferentCalls_ReturnsDifferentValues()
    {
        var (hash1, salt1) = _sut.GenerateHashAndSalt(testPw);
        var (hash2, salt2) = _sut.GenerateHashAndSalt(testPw);

        Assert.NotEqual(hash1, hash2);
        Assert.NotEqual(salt1, salt2);
    }
    [Fact]
    public void GenerateHashAndSalt_NullPassword_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => _sut.GenerateHashAndSalt(null));
    }
    [Fact]
    public void VerifyPassword_CorrectValues_ReturnsTrue()
    {
        var (hash, salt) = _sut.GenerateHashAndSalt(testPw);

        var result = _sut.VerifyPassword(hash, testPw, salt);

        Assert.True(result);
    }
    [Fact]
    public void VerifyPassword_WrongValues_ReturnsFalse()
    {
        var (hash, salt) = _sut.GenerateHashAndSalt(testPw);

        var result = _sut.VerifyPassword(hash, "differentPw", salt);

        Assert.False(result);
    }
    

}
