using Inventory_Order_Tracking.API.Context;
using Inventory_Order_Tracking.API.Models;
using Inventory_Order_Tracking.API.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Inventory_Order_Tracking.API.Repository
{
    /// <summary>
    /// Provides data access and persistence operations for <see cref="Product"/> entities.
    /// </summary>
    /// <remarks>
    /// Implements the <see cref="IProductRepository"/> interface to interact with the database
    /// using Entity Framework Core.
    /// </remarks>
    public class ProductRepository(InventoryManagementContext context) : IProductRepository
    {
        /*
        No update method - handled on service level
        */

        /// <inheritdoc/>
        public async Task<List<Product>> GetAllAsync()
        {
            return await context.Products.ToListAsync();
        }

        /// <inheritdoc/>
        public async Task<Product?> GetByIdAsync(Guid productId)
        {
            return await context.Products.FindAsync(productId);
        }

        /// <inheritdoc/>
        public async Task<Product> AddAsync(Product entity)
        {
            var result = await context.Products.AddAsync(entity);
            await context.SaveChangesAsync();
            return entity;
        }

        /// <inheritdoc/>
        public async Task DeleteAsync(Product entity)
        {
            context.Products.Remove(entity);
            await context.SaveChangesAsync();
        }

        /// <inheritdoc/>
        public async Task SaveChangesAsync()
        {
            await context.SaveChangesAsync();
        }
    }
}