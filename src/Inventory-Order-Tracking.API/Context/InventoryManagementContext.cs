using Inventory_Order_Tracking.API.Models;
using Microsoft.EntityFrameworkCore;

namespace Inventory_Order_Tracking.API.Context
{
    public class InventoryManagementContext(DbContextOptions options) : DbContext(options)
    {
        public required DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasKey(x => x.Id);
        }
    }
   
}
