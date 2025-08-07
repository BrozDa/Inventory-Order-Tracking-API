
namespace Inventory_Order_Tracking.API.Services.Interfaces
{
    /// <summary>
    /// Provides contract for seeding the data storage with initial testing data
    /// </summary>
    public interface ISeedingService
    {
        /// <summary>
        /// Generates and stores initial testing data
        /// </summary>
        Task SeedInitialData();
    }
}