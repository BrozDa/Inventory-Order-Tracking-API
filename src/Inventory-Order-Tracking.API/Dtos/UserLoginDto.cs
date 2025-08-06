namespace Inventory_Order_Tracking.API.Dtos
{

    /// <summary>
    /// Represents the data transfer object for user login.
    /// </summary>
    public class UserLoginDto
    {
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}