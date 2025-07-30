
using Inventory_Order_Tracking.API.Configuration;
using Inventory_Order_Tracking.API.Context;
using Inventory_Order_Tracking.API.Repository;
using Inventory_Order_Tracking.API.Repository.Interfaces;
using Inventory_Order_Tracking.API.Services;
using Inventory_Order_Tracking.API.Services.Interfaces;
using Inventory_Order_Tracking.API.Utils;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using System.Text;
using FluentValidation;
using FluentValidation.Results;
using Inventory_Order_Tracking.API.Validators;
using Microsoft.Extensions.Options;

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

            var jwtSettingValidator = new JwtSettingValidator();
           
            try
            {

                var builder = WebApplication.CreateBuilder(args);

                builder.Host.UseSerilog();
                builder.Services.AddEndpointsApiExplorer();
                builder.Services.AddSwaggerGen();

                var jwtSettingsSection = builder.Configuration.GetSection("JwtSettings");

                var jwtSettings = jwtSettingsSection.Get<JwtSettings>();

                Console.WriteLine("1");
                Console.WriteLine(jwtSettings is not null);
                Console.WriteLine("2");
                if (jwtSettings is null)
                    throw new ArgumentNullException("Missing JwtSettings in Appconfig.js");

                
                var validator = new JwtSettingValidator();


                var result = validator.Validate(jwtSettings);
                
                if (!result.IsValid)
                {
  
                    var errors = string.Join("; ", result.Errors.Select(e => e.ErrorMessage));
                    throw new ArgumentException(errors);
                }

                Console.WriteLine("2");
                builder.Services.AddSingleton(jwtSettings);

                builder.Services.AddScoped<IAuthService, AuthService>();
                builder.Services.AddScoped<RegisterRequestValidator>();
                builder.Services.AddScoped<IUserRepository, UserRepository>();
                builder.Services.AddDbContext<InventoryManagementContext>(options =>
                    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
                );
 
                builder.Services.AddControllers();
                builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidIssuer = jwtSettings.Issuer,
                        ValidateAudience = true,

                        ValidAudience = jwtSettings.Audience,
                        ValidateLifetime = true,

                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(
                            Encoding.UTF8.GetBytes(jwtSettings.Token)),
                    };
                }
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
