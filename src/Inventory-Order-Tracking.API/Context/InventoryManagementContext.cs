using Inventory_Order_Tracking.API.Domain;
using Inventory_Order_Tracking.API.Models;
using Inventory_Order_Tracking.API.Utils;
using Microsoft.EntityFrameworkCore;

namespace Inventory_Order_Tracking.API.Context
{
    public class InventoryManagementContext(DbContextOptions options) : DbContext(options)
    {
        public required DbSet<User> Users { get; set; }
        public required DbSet<EmailVerificationToken> EmailVerificationTokens { get; set; }

        public required DbSet<Product> Products { get; set; }
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

            modelBuilder.Entity<Product>(entity =>
            {
                entity.HasKey(t => t.Id);
            });
        }
        public async Task SeedAdminUserAsync()
        {
            var (hash, salt) = PasswordHasher.GenerateHashAndSalt("admin");
            var user = new User
            {
                Id = Guid.NewGuid(),
                Role = UserRoles.Admin,
                Username = "admin",
                PasswordHash = hash,
                PasswordSalt = salt,
                Email = "admin@admin.com",
                IsVerified = true,
            };
            await Users.AddAsync(user);
            await SaveChangesAsync();
        }

    
    }
   
}
