namespace Inventory_Order_Tracking.API.Configuration
{
    /// <summary>
    /// Represents settings for Jwt token stored in appconfig.json
    /// </summary>
    public class JwtSettings
    {
        public string Token { get; set; } = string.Empty;
        public string Issuer { get; set; } = string.Empty;

        public string Audience { get; set; } = string.Empty;

        public int TokenExpirationDays { get; set; }

        public int RefreshTokenExpirationDays { get; set; }
    }
}