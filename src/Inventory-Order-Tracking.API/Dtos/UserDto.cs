namespace Inventory_Order_Tracking.API.Dtos
{

    /// <summary>
    /// Represents the data transfer object for fetching existing user.
    /// </summary>
    public class UserDto
    {
        public Guid Id { get; set; }
        public bool IsValidated { get; set; }
        public string Role { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
    }
}