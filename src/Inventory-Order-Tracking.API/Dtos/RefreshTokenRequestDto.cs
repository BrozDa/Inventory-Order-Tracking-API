namespace Inventory_Order_Tracking.API.Dtos
{
    public class RefreshTokenRequestDto
    {
        public Guid UserId { get; set; }
        public string ExpiredRefreshToken { get; set; }
    }
}