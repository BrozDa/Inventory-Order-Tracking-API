using Inventory_Order_Tracking.API.Context;
using Inventory_Order_Tracking.API.Services;
using Inventory_Order_Tracking.API.Installers;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace Inventory_Order_Tracking.API
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            LoggerInstaller.AddLogger();

            try
            {
                var builder = WebApplication.CreateBuilder(args);

                builder.Host.UseSerilog();

                builder.Services.AddAuth(builder.Configuration);
                builder.Services.AddEmail(builder.Configuration);
                builder.Services.AddDbContext(builder.Configuration);
                builder.Services.AddServices();


                var app = builder.Build();

                if (app.Environment.IsDevelopment())
                {
                    app.UseSwagger();
                    app.UseSwaggerUI();

                    await app.SeedDatabaseAsync();    
                }

                app.AddMiddleware();

                app.Run();

            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Application failed to start");
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }
    }
}