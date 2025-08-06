namespace Inventory_Order_Tracking.API.Models
{
    /// <summary>
    /// Represents an email verification token user for verification of newly registered user
    /// </summary>
    public class EmailVerificationToken
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public User User { get; set; } = null!;
        public DateTime CreatedOn { get; set; }
        public DateTime ExpiresOn { get; set; }
    }
}