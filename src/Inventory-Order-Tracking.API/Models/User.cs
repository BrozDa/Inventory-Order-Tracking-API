namespace Inventory_Order_Tracking.API.Models
{
    public class User
    {
        public Guid Id { get; set; }
        public string Role { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public string PasswordSalt { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public bool IsVerified { get; set; }
        public string? RefreshToken { get; set; }
        public DateTime? RefreshTokenExpirationTime { get; set; }

        public ICollection<EmailVerificationToken> EmailVerificationTokens { get; set; } = new List<EmailVerificationToken>();
        public ICollection<Order> Orders { get; set; } = new List<Order>();
        public ICollection<AuditLog> AuditLogs { get; set; } = new List<AuditLog>();
    }
}