namespace Inventory_Order_Tracking.API.Dtos
{
    /// <summary>
    /// Represents the data transfer object for requesting new refresh token for the user.
    /// </summary>
    public class RefreshTokenRequestDto
    {
        public Guid UserId { get; set; }
        public string ExpiredRefreshToken { get; set; } = string.Empty;
    }
}