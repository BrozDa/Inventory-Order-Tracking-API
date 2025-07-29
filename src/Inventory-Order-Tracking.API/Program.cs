
using Inventory_Order_Tracking.API.Context;
using Inventory_Order_Tracking.API.Controllers;
using Inventory_Order_Tracking.API.Services;
using Inventory_Order_Tracking.API.Services.Interfaces;
using Inventory_Order_Tracking.API.Utils;
using Inventory_Order_Tracking.API.Validators;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace Inventory_Order_Tracking.API
{
    public class Program
    {
        public static void Main(string[] args)
        {

            Log.Logger = new LoggerConfiguration()
                .WriteTo.File(
                    "logs/log.txt", 
                    rollingInterval: RollingInterval.Day,
                    outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}")
                .MinimumLevel.Information()
                .CreateLogger();

           
            try
            {

                var builder = WebApplication.CreateBuilder(args);

                builder.Host.UseSerilog();

                builder.Services.AddEndpointsApiExplorer();
                builder.Services.AddSwaggerGen();
                
                builder.Services.AddScoped<IAuthService, AuthService>();
                builder.Services.AddScoped<RegisterRequestValidator>();
                builder.Services.AddDbContext<InventoryManagementContext>(options =>
                    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
                );
                builder.Services.AddControllers();


                var app = builder.Build();

                if (app.Environment.IsDevelopment())
                {
                    app.UseSwagger();
                    app.UseSwaggerUI();
                }
                app.UseHttpsRedirection();

                app.UseAuthorization();


                app.MapControllers();
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
