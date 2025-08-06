using Inventory_Order_Tracking.API.Configuration;
using Inventory_Order_Tracking.API.Context;
using Inventory_Order_Tracking.API.Repository.Interfaces;
using Inventory_Order_Tracking.API.Repository;
using Inventory_Order_Tracking.API.Services.Interfaces;
using Inventory_Order_Tracking.API.Services;
using Inventory_Order_Tracking.API.Utils;
using Inventory_Order_Tracking.API.Validators;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Inventory_Order_Tracking.API.Domain;

namespace Inventory_Order_Tracking.API.Installers
{
    public static class ServicesInstaller
    {
        public static IServiceCollection AddAuth(this IServiceCollection services, IConfiguration configuration)
        {
            var jwtSettingsSection = configuration.GetSection("JwtSettings");

            var jwtSettings = jwtSettingsSection.Get<JwtSettings>();

            if (jwtSettings is null)
                throw new ArgumentNullException("Missing JwtSettings in appsettings.json");

            var validator = new JwtSettingsValidator();

            var validationResult = validator.Validate(jwtSettings);

            if (!validationResult.IsValid)
            {
                var errors = string.Join("; ", validationResult.Errors.Select(e => e.ErrorMessage));
                throw new ArgumentException(errors);
            }

            services.AddSingleton(jwtSettings);

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
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
            services.AddAuthorization(options =>
            {
                options.AddPolicy("admin", policy => policy.RequireRole(UserRoles.Admin));
                options.AddPolicy("customer", policy => policy.RequireRole(UserRoles.Admin, UserRoles.Customer));
            });

            return services;
        }
        public static IServiceCollection AddEmail(this IServiceCollection services, IConfiguration configuration)
        {
            var emailSettingsSection = configuration.GetSection("EmailSettings");

            var emailSettings = emailSettingsSection.Get<EmailSettings>();

            if (emailSettings is null)
                throw new ArgumentNullException("Missing EmailSettings in appsettings.json");

            var emailSettingsValidator = new EmailSettingsValidator();

            var validationResult = emailSettingsValidator.Validate(emailSettings);

            if (!validationResult.IsValid)
            {
                var errors = string.Join("; ", validationResult.Errors.Select(e => e.ErrorMessage));
                throw new ArgumentException(errors);
            }

            services.AddSingleton(emailSettings);
            services.AddFluentEmail(emailSettings.SenderEmail, emailSettings.Sender)
                    .AddSmtpSender(emailSettings.Host, emailSettings.Port);

            return services;

        }

        public static IServiceCollection AddDbContext(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<InventoryManagementContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"))
            );

            return services;
        }
        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();

            services.AddHttpContextAccessor();

            services.AddControllers();

            services.AddScoped<IEmailVerificationService, EmailVerificationService>();
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IOrderService, OrderService>();
            services.AddScoped<ICurrentUserService, CurrentUserService>();
            services.AddScoped<IAuditLogService, AuditLogService>();

            services.AddScoped<IEmailVerificationTokenRepository, EmailVerificationTokenRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<IOrderRepository, OrderRepository>();
            services.AddScoped<IAuditLogRepository, AuditLogRepository>();


            services.AddScoped<RegisterRequestValidator>();
            services.AddScoped<ProductAddValidator>();
            services.AddScoped<ProductUpdateValidator>();
            services.AddScoped<ProductUpdateNameValidator>();
            services.AddScoped<ProductUpdateDescriptionValidator>();

            return services;

            
        }
    }
}
