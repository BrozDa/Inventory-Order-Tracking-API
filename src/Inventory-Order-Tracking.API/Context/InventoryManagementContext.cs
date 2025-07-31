using Inventory_Order_Tracking.API.Models;
using Microsoft.EntityFrameworkCore;

namespace Inventory_Order_Tracking.API.Context
{
    public class InventoryManagementContext(DbContextOptions options) : DbContext(options)
    {
        public required DbSet<User> Users { get; set; }
        public required DbSet<EmailVerificationToken> EmailVerificationTokens { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(u => u.Id);

                entity.HasIndex(u => u.Username).IsUnique();

                entity.HasIndex(u => u.Email).IsUnique();
            });

            modelBuilder.Entity<EmailVerificationToken>(entity =>
            {
                entity.HasKey(t => t.Id);
                entity.HasOne(t => t.User).WithMany().HasForeignKey(t => t.UserId);
            });
        }
    }
   
}
