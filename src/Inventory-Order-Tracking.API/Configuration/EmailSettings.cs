namespace Inventory_Order_Tracking.API.Configuration
{
    public class EmailSettings
    {
        public string SenderEmail { get; set; } = string.Empty;
        public string Sender { get; set; } = string.Empty;
        public string Host { get; set; } = string.Empty;
        public int Port { get; set; }
    }
}