using Inventory_Order_Tracking.API.Context;
using Inventory_Order_Tracking.API.Services;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace Inventory_Order_Tracking.API.Installers
{
    public static class DevEnvSeeder
    {
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
