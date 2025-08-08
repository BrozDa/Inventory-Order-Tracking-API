using Inventory_Order_Tracking.API.Configuration;
using Inventory_Order_Tracking.API.Context;
using Inventory_Order_Tracking.API.Domain;
using Inventory_Order_Tracking.API.Repository;
using Inventory_Order_Tracking.API.Repository.Interfaces;
using Inventory_Order_Tracking.API.Services;
using Inventory_Order_Tracking.API.Services.Interfaces;
using Inventory_Order_Tracking.API.Utils;
using Inventory_Order_Tracking.API.Validators;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Inventory_Order_Tracking.API.Installers
{
    /// <summary>
    /// Provides a service installing extension for the <see cref="IServiceCollection"/>.
    /// </summary>
    public static class ServicesInstaller
    {
        /// <summary>
        /// Validates Jwt settings from provided <see cref="IConfiguration"/> and adds JWT settings, Authentication and Authorization to the service collection
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/> to extend</param>
        /// <param name="configuration">An instance of <see cref="IConfiguration"/> used to read configuration data</param>
        /// <returns>Extended instance of the <see cref="IServiceCollection"/></returns>
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

        /// <summary>
        /// Validates email settings from provided <see cref="IConfiguration"/> and adds email settings and AddFluentEmail to the service collection
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/> to extend</param>
        /// <param name="configuration">An instance of <see cref="IConfiguration"/> used to read configuration data</param>
        /// <returns>Extended instance of the <see cref="IServiceCollection"/></returns>
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

        /// <summary>
        /// Gets the connection string from provided <see cref="IConfiguration"/> and adds Db context to the service collection
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/> to extend</param>
        /// <param name="configuration">An instance of <see cref="IConfiguration"/> used to read configuration data</param>
        /// <returns>Extended instance of the <see cref="IServiceCollection"/></returns>
        public static IServiceCollection AddDbContext(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<InventoryManagementContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"))
            );

            return services;
        }

        /// <summary>
        /// Adds services used across the app to the service collection
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/> to extend</param>
        /// <returns>Extended instance of the <see cref="IServiceCollection"/></returns>
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