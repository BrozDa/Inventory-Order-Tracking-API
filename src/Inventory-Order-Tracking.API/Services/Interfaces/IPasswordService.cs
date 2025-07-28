namespace Inventory_Order_Tracking.API.Services.Interfaces
{
    public interface IPasswordService
    {
        (string hash, string salt) GenerateHashAndSalt(string password);
        bool VerifyPassword(string hash, string password, string salt);
    }
}
