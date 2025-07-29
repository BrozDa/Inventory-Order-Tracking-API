
using Inventory_Order_Tracking.API.Context;
using Inventory_Order_Tracking.API.Controllers;
using Inventory_Order_Tracking.API.Utils;
using Microsoft.EntityFrameworkCore;

namespace Inventory_Order_Tracking.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddScoped<PasswordValidator>();
            builder.Services.AddDbContext<InventoryManagementContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
            );

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
    }
}
