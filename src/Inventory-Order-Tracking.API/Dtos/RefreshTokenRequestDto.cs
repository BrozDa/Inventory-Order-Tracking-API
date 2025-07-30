namespace Inventory_Order_Tracking.API.Dtos
{
    public class RefreshTokenRequestDto
    {
        public required Guid UserId { get; set; }
        public required string ExpiredRefreshToken { get; set; }
    }
}
