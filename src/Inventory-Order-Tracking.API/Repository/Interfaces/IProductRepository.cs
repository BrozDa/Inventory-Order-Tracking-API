using Inventory_Order_Tracking.API.Models;

namespace Inventory_Order_Tracking.API.Repository.Interfaces
{
    public interface IProductRepository
    {
        Task<Product> AddAsync(Product entity);
        Task<bool> DeleteAsync(Guid id);
        Task<List<Product>> GetAllAsync();
        Task<Product?> GetByIdAsync(Guid productId);
        Task SaveChangesAsync();
    }
}