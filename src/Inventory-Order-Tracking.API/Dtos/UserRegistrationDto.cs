namespace Inventory_Order_Tracking.API.Dtos
{

    /// <summary>
    /// Represents the data transfer object for user registration.
    /// </summary>
    public class UserRegistrationDto
    {
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
    }
}