using Inventory_Order_Tracking.API.Models;
using Microsoft.EntityFrameworkCore;

namespace Inventory_Order_Tracking.API.Context
{
    /// <summary>
    /// Represents the Entity Framework Core database context for the Taskify application.
    /// </summary>
    /// <param name="options">The options used to configure the context.</param>
    public class InventoryManagementContext(DbContextOptions options) : DbContext(options)
    {
        public DbSet<User> Users { get; set; } = null!;
        public DbSet<EmailVerificationToken> EmailVerificationTokens { get; set; } = null!;
        public DbSet<Product> Products { get; set; } = null!;

        public DbSet<Order> Orders { get; set; } = null!;
        public DbSet<OrderItem> OrderItems { get; set; } = null!;
        public DbSet<AuditLog> AuditLog { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //sets 1-to-many relationship between User-Orders, User-AuditLogs and
            //User-EmailVerifiationTokens
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

                entity.HasMany(u => u.EmailVerificationTokens)
                    .WithOne(evt => evt.User)
                    .HasForeignKey(evt => evt.UserId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<EmailVerificationToken>().HasKey(t => t.Id);

            //setups 1-to-many relationship between Product-OrderItems
            modelBuilder.Entity<Product>(entity =>
            {
                entity.HasKey(p => p.Id);

                entity.HasMany(p => p.OrderItems)
                    .WithOne(oi => oi.Product)
                    .HasForeignKey(oi => oi.ProductId);
            });
            //setups 1-to-many relationship between Order-OrderItems
            modelBuilder.Entity<Order>(entity =>
            {
                entity.HasKey(o => o.Id);

                entity.HasMany(o => o.Items)
                    .WithOne(oi => oi.Order)
                    .HasForeignKey(oi => oi.OrderId);
            });

            modelBuilder.Entity<OrderItem>().HasKey(oi => oi.Id);
        }
    }
}