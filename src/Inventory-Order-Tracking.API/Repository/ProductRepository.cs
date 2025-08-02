using Inventory_Order_Tracking.API.Context;
using Inventory_Order_Tracking.API.Models;
using Microsoft.EntityFrameworkCore;

namespace Inventory_Order_Tracking.API.Repository
{
    public class ProductRepository(InventoryManagementContext context) : IProductRepository
    {

        /*
        No update method - handled on service level
        */
        public async Task<List<Product>> GetAllAsync()
        {
            return await context.Products.ToListAsync();
        }
        public async Task<Product?> GetByIdAsync(Guid productId)
        {
            return await context.Products.FindAsync(productId);
        }

        public async Task<Product> AddAsync(Product entity)
        {
            var result = await context.Products.AddAsync(entity);
            await context.SaveChangesAsync();
            return entity;
        }
        public async Task<bool> DeleteAsync(Guid id)
        {
            var toBeDeleted = await GetByIdAsync(id);

            if (toBeDeleted is null)
                return false;

            context.Products.Remove(toBeDeleted);
            await context.SaveChangesAsync();
            return true;
        }
        public async Task SaveChangesAsync()
        {
            await context.SaveChangesAsync();
        }


    }
}
