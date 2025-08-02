
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
using Inventory_Order_Tracking.API.Validators;
using Inventory_Order_Tracking.API.Domain;

namespace Inventory_Order_Tracking.API
{
    public class Program
    {
        public static async Task Main(string[] args)
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

                /////////////////////////////////////////////////////////////////////////////////// VALIDATE JWT SETTINGS
                var jwtSettingsSection = builder.Configuration.GetSection("JwtSettings");

                var jwtSettings = jwtSettingsSection.Get<JwtSettings>();

                if (jwtSettings is null)
                    throw new ArgumentNullException("Missing JwtSettings in Appconfig.js");

                var validator = new JwtSettingValidator();

                var result = validator.Validate(jwtSettings);
                
                if (!result.IsValid)
                {
  
                    var errors = string.Join("; ", result.Errors.Select(e => e.ErrorMessage));
                    throw new ArgumentException(errors);
                }

                /////////////////////////////////////////////////////////////////////////////////// VALIDATE EMAIL SETTINGS
                var emailSettingsSection = builder.Configuration.GetSection("EmailSettings");

                var emailSettings = emailSettingsSection.Get<EmailSettings>();

                if (emailSettings is null)
                    throw new ArgumentNullException("Missing EmailSettings in Appconfig.js");


                var emailSettingsValidator = new EmailSettingsValidator();

                var emailSettingsResult = validator.Validate(jwtSettings);

                if (!result.IsValid)
                {
                    var errors = string.Join("; ", result.Errors.Select(e => e.ErrorMessage));
                    throw new ArgumentException(errors);
                }

                /////////////////////////////////////////////////////////////////////////////////// REGISTER BOTH SETTINGS
                builder.Services.AddSingleton(jwtSettings);
                builder.Services.AddSingleton(emailSettings);


                /////////////////////////////////////////////////////////////////////////////////////// Rest of services
                builder.Services
                    .AddFluentEmail(emailSettings.SenderEmail, emailSettings.Sender)
                    .AddSmtpSender(emailSettings.Host,  emailSettings.Port);

                builder.Services.AddHttpContextAccessor();

                builder.Services.AddScoped<IEmailVerificationService, EmailVerificationService>();
                builder.Services.AddScoped<IProductService, ProductService>();
                builder.Services.AddScoped<IAuthService, AuthService>();
                
                builder.Services.AddScoped<IEmailVerificationTokenRepository, EmailVerificationTokenRepository>();
                builder.Services.AddScoped<IUserRepository, UserRepository>();
                builder.Services.AddScoped<IProductRepository, ProductRepository>();

                builder.Services.AddScoped<RegisterRequestValidator>();

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
                });
                builder.Services.AddAuthorization(options =>
                {
                    options.AddPolicy("admin", policy => policy.RequireRole(UserRoles.Admin));
                    options.AddPolicy("customer", policy => policy.RequireRole(UserRoles.Admin, UserRoles.Customer));
                });

                var app = builder.Build();
                
                using var scope = app.Services.CreateScope();
                var context = scope.ServiceProvider.GetRequiredService<InventoryManagementContext>();
                await context.Database.MigrateAsync();
                //await context.SeedAdminUserAsync();

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
