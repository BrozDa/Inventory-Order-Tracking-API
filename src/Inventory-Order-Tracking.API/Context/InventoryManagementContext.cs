using Inventory_Order_Tracking.API.Domain;
using Inventory_Order_Tracking.API.Models;
using Inventory_Order_Tracking.API.Utils;
using Microsoft.EntityFrameworkCore;

namespace Inventory_Order_Tracking.API.Context
{
    public class InventoryManagementContext(DbContextOptions options) : DbContext(options)
    {
        public DbSet<User> Users { get; set; }
        public DbSet<EmailVerificationToken> EmailVerificationTokens { get; set; }
        public DbSet<Product> Products { get; set; }

        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<AuditLog> AuditLog { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(u => u.Id);

                entity.HasIndex(u => u.Username).IsUnique();

                entity.HasIndex(u => u.Email).IsUnique();

                entity.HasMany(u => u.Orders)
                    .WithOne(o => o.User)
                    .HasForeignKey(o => o.UserId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasMany(u => u.AuditLogs)
                    .WithOne(al => al.User)
                    .HasForeignKey(al => al.UserId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<EmailVerificationToken>(entity =>
            {
                entity.HasKey(t => t.Id);
                entity.HasOne(t => t.User).WithMany().HasForeignKey(t => t.UserId);
            });

            modelBuilder.Entity<Product>(entity =>
            {
                entity.HasKey(p => p.Id);

                entity.HasMany(p => p.OrderItems)
                .WithOne(oi => oi.Product)
                .HasForeignKey(oi => oi.ProductId);
            });

            modelBuilder.Entity<Order>(entity =>
            {
                entity.HasKey(o => o.Id);

                entity.HasMany(o => o.Items)
                .WithOne(oi => oi.Order)
                .HasForeignKey(oi => oi.OrderId);
            });
            modelBuilder.Entity<OrderItem>().HasKey(oi => oi.Id);
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