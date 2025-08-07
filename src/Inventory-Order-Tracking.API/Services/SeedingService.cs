using Inventory_Order_Tracking.API.Context;
using Inventory_Order_Tracking.API.Domain;
using Inventory_Order_Tracking.API.Models;
using Inventory_Order_Tracking.API.Services.Interfaces;
using Inventory_Order_Tracking.API.Utils;
using Microsoft.EntityFrameworkCore;

namespace Inventory_Order_Tracking.API.Services
{
    /// <summary>
    /// Provides operations for seeding the data storage with initial testing data
    /// </summary>
    public class SeedingService(
        InventoryManagementContext context) : ISeedingService
    {
        /// <inheritdoc/>
        public async Task SeedInitialData()
        {
            if (await context.Users.AnyAsync())
                return;

            var admin = await SeedAdminUser();
            var customer = await SeedCustomerUser();
            var products = await SeedProducts();
            var orders = await SeedOrders(admin, customer);
            var orderItems = await SeedOrderItems(orders, products);
            var logs = await SeedAuditLogs(admin, customer);

            await context.SaveChangesAsync();
        }
        /// <summary>
        /// Generates and stores admin user to the database
        /// </summary>
        /// <returns>An instance of <see cref="User"/> with generated admin user</returns>
        private async Task<User> SeedAdminUser()
        {
            var (hash, salt) = PasswordHasher.GenerateHashAndSalt("admin");
            var user = new User
            {
                Role = UserRoles.Admin,
                Username = "admin",
                PasswordHash = hash,
                PasswordSalt = salt,
                Email = "admin@admin.com",
                IsVerified = true,
            };

            await context.Users.AddAsync(user);

            return user;

        }

        /// <summary>
        /// Generates and stores customer user to the database
        /// </summary>
        /// <returns>An instance of <see cref="User"/> with generated customer user</returns>
        private async Task<User> SeedCustomerUser()
        {
            var (hash, salt) = PasswordHasher.GenerateHashAndSalt("customer");
            var user = new User
            {
                Role = UserRoles.Customer,
                Username = "customer",
                PasswordHash = hash,
                PasswordSalt = salt,
                Email = "customer@customer.com",
                IsVerified = true,
            };
            await context.Users.AddAsync(user);

            return user;
        }

        /// <summary>
        /// Generates and stores Products to the database
        /// </summary>
        /// <returns>A list of generated <see cref="Product"/> instances</returns>
        private async Task<List<Product>> SeedProducts()
        {
            var products = new List<Product>()
            {
                new Product{
                    Name = "Vacuum cleaner",
                    Description = "Best cleaner you find on the market",
                    StockQuantity = 13,
                    Price = 49.99m
                },
                new Product{
                    Name = "Laptop Ultra",
                    Description = "New guts so you can loose your match faster",
                    StockQuantity = 3,
                    Price = 179.99m
                },
                new Product{
                    Name = "Air Fryer",
                    Description = "No need for a pan anymore",
                    StockQuantity = 7,
                    Price = 35.99m
                },
                new Product{
                    Name = "Mobile Phone Max",
                    Description = "New phone to put to silent mode til word colapses",
                    StockQuantity = 21,
                    Price = 249.99m
                },
                new Product{
                    Name = "Ultra wide TV",
                    Description = "Wider than your future",
                    StockQuantity = 4,
                    Price = 79.99m
                }
            };
            await context.Products.AddRangeAsync(products);

            return products;
        }

        /// <summary>
        /// Generates and stores products Orders to the database
        /// </summary>
        /// <param name="admin">An instance of admin user</param>
        /// <param name="customer">An instance of customer user</param>
        /// <returns>A list of generated <see cref="Order"/> instances</returns>
        private async Task<List<Order>> SeedOrders(User admin, User customer)
        {
            var orders = new List<Order>()
            {
                new Order{ //1x Vacuum cleaner
                    UserId = customer.Id,
                    Status = OrderStatus.Submitted,
                    OrderDate = DateTime.UtcNow.AddDays(-2),
                    OrderPrice = 49.99m
                },
                new Order{ // 1x Laptop Ultra, 2x Air Fryer, 1x Ultra wide TV
                    UserId = customer.Id,
                    Status = OrderStatus.Completed,
                    OrderDate = DateTime.UtcNow.AddDays(-8),
                    OrderPrice = 331.96m
                },
                new Order{ //8x Mobile Phone Max
                    UserId = admin.Id,
                    Status = OrderStatus.Cancelled,
                    OrderDate = DateTime.UtcNow.AddDays(-16),
                    OrderPrice = 1999.92m
                }
            };
            await context.Orders.AddRangeAsync(orders);

            return orders;
        }

        /// <summary>
        /// Generates and stores products order items to the database
        /// </summary>
        /// <param name="orders">A list of previously generated orders</param>
        /// <param name="products">A list of previously generated products</param>
        /// <returns>A list of generated <see cref="OrderItem"/> instances</returns>
        private async Task<List<OrderItem>> SeedOrderItems(List<Order> orders, List<Product> products)
        {
            var orderItems = new List<OrderItem>()
            {
                new OrderItem{
                    OrderId = orders[0].Id,
                    ProductId = products[0].Id,
                    OrderedQuantity = 1,
                    UnitPrice = 49.99m
                },

                new OrderItem{
                    OrderId = orders[1].Id,
                    ProductId = products[1].Id,
                    OrderedQuantity = 1,
                    UnitPrice = 179.99m
                },
                new OrderItem{
                    OrderId = orders[1].Id,
                    ProductId = products[2].Id,
                    OrderedQuantity = 2,
                    UnitPrice = 35.99m
                },
                new OrderItem{
                    OrderId = orders[1].Id,
                    ProductId = products[4].Id,
                    OrderedQuantity = 1,
                    UnitPrice = 79.99m
                },

                new OrderItem{
                    OrderId = orders[2].Id,
                    ProductId = products[3].Id,
                    OrderedQuantity = 8,
                    UnitPrice = 249.99m
                }
            };
            await context.OrderItems.AddRangeAsync(orderItems);
            return orderItems;
        }

        /// <summary>
        /// Generates and stores products audit logs items to the database
        /// </summary>
        /// <param name="admin">An instance of admin user</param>
        /// <param name="customer">An instance of customer user</param>
        /// <returns></returns>
        private async Task<List<AuditLog>> SeedAuditLogs(User admin, User customer)
        {
            var logs = new List<AuditLog>()
            {
                new AuditLog
                {
                    UserId = customer.Id,
                    Timestamp = DateTime.UtcNow.AddDays(-7),
                    Action = "Registered"
                },

                new AuditLog
                {
                    UserId = admin.Id,
                    Timestamp = DateTime.UtcNow.AddDays(-2),
                    Action = "Changed product Price"
                },
                new AuditLog
                {
                    UserId = admin.Id,
                    Timestamp = DateTime.UtcNow.AddDays(-2),
                    Action = "Changed product Name"
                },
                new AuditLog
                {
                    UserId = admin.Id,
                    Timestamp = DateTime.UtcNow.AddDays(-2),
                    Action = "Added new product"
                },
                new AuditLog
                {
                    UserId = admin.Id,
                    Timestamp = DateTime.UtcNow.AddDays(-3),
                    Action = "Deleted product"
                },
            };
            
            await context.AuditLog.AddRangeAsync(logs);

            return logs;

        }

    }
}
