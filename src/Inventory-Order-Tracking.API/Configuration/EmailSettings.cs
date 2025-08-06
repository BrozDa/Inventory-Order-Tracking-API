namespace Inventory_Order_Tracking.API.Configuration
{
    /// <summary>
    /// Represents settings for fluent email stored in appconfig.json
    /// </summary>
    public class EmailSettings
    {
        public string SenderEmail { get; set; } = string.Empty;
        public string Sender { get; set; } = string.Empty;
        public string Host { get; set; } = string.Empty;
        public int Port { get; set; }
    }
}