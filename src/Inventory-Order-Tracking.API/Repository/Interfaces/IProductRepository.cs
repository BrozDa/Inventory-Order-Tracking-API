using Inventory_Order_Tracking.API.Models;

namespace Inventory_Order_Tracking.API.Repository.Interfaces
{
    public interface IProductRepository
    {
        Task<Product> AddAsync(Product entity);

        Task DeleteAsync(Product entity);

        Task<List<Product>> GetAllAsync();

        Task<Product?> GetByIdAsync(Guid productId);

        Task SaveChangesAsync();
    }
}