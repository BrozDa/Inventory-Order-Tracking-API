using Inventory_Order_Tracking.API.Models;
using Microsoft.EntityFrameworkCore;

namespace Inventory_Order_Tracking.API.Context
{
    public class InventoryManagementContext(DbContextOptions options) : DbContext(options)
    {
        public required DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(u => u.Id);

                entity.HasIndex(u => u.Username).IsUnique();

                entity.HasIndex(u => u.Email).IsUnique();
            });
        }
    }
   
}
