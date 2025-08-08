using Inventory_Order_Tracking.API.Context;
using Inventory_Order_Tracking.API.Services;
using Microsoft.EntityFrameworkCore;

namespace Inventory_Order_Tracking.API.Installers
{
    /// <summary>
    /// Provides a middleware installing extension for the <see cref="WebApplication"/>.
    /// </summary>
    public static class DataSeeder
    {
        /// <summary>
        /// Deletes the existing database, applies all migrations, and seeds initial data.
        /// </summary>
        /// <param name="app">The <see cref="WebApplication"/> instance to extend.</param>
        public static async Task SeedDatabaseAsync(this WebApplication app)
        {
            using var scope = app.Services.CreateScope();

            var context = scope.ServiceProvider.GetRequiredService<InventoryManagementContext>();
            var seeder = new SeedingService(context);

            await context.Database.EnsureDeletedAsync();
            await context.Database.MigrateAsync();
            await seeder.SeedInitialData();
        }
    }
}