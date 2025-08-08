using Inventory_Order_Tracking.API.Domain;
using Inventory_Order_Tracking.API.Dtos;
using Inventory_Order_Tracking.API.Models;
using Inventory_Order_Tracking.API.Repository.Interfaces;
using Inventory_Order_Tracking.API.Services.Interfaces;
using Inventory_Order_Tracking.API.Services.Shared;

namespace Inventory_Order_Tracking.API.Services
{
    /// <summary>
    /// Provides operations related to handling and managing orders.
    /// </summary>
    public class OrderService(
        IAuditLogService auditService,
        IUserRepository userRepo,
        IProductRepository productRepo,
        IOrderRepository orderRepo,
        ILogger<OrderService> logger) : IOrderService
    {
        /// <inheritdoc/>
        public async Task<ServiceResult<OrderDto>> SubmitOrderAsync(Guid userId, OrderCreateDto dto)
        {
            try
            {
                var user = await userRepo.GetByIdAsync(userId);

                if (user is null)
                {
                    logger.LogWarning("[OrderService][SubmitOrderAsync] Non existent user attempted to submit order");

                    return ServiceResult<OrderDto>.Failure(
                        errors: ["Non existent user"],
                        statusCode: 400);

                }
                var (orderedProducts, errors) = await ValidateAndFetchProducts(dto);

                if (errors.Count > 0)
                {
                    logger.LogWarning("[OrderService][SubmitOrderAsync] Invalid products within an order by {user}, encountered errors:" +
                        "{errors}", userId, errors);

                    return ServiceResult<OrderDto>.Failure(
                        errors: errors,
                        statusCode: 400);
                }

                var order = await CreateOrder(userId, orderedProducts);

                await orderRepo.SaveChangesAsync(); // saves even products due to shared context

                await auditService.AddNewLogAsync(
                    new AuditLogAddDto
                    {
                        UserId = userId,
                        Action = $"Placed order {order.Id}"
                    });

                return ServiceResult<OrderDto>.Success(
                    data: order.ToDto(),
                    statusCode: 201);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "[OrderService][SubmitOrderAsync] Unhandled Exception has occured");

                return ServiceResult<OrderDto>.Failure(
                    errors: ["Failed to submit order"],
                    statusCode: 500);
            }
        }

        /// <inheritdoc/>
        public async Task<ServiceResult<OrderDto>> GetOrderByIdAsync(Guid userId, Guid orderId)
        {
            try
            {
                var user = await userRepo.GetByIdAsync(userId);

                if (user is null)
                {
                    logger.LogWarning("[OrderService][GetOrderByIdAsync] Non existent user attempted to fetch order");
                    return ServiceResult<OrderDto>.Failure(
                        errors: ["Non existent user"],
                        statusCode: 404);
                }

                var order = await orderRepo.GetByIdAsync(orderId);

                if (order is null)
                {
                    logger.LogWarning("[OrderService][GetOrderByIdAsync] Non existent order fetch attempt");
                    return ServiceResult<OrderDto>.Failure(
                        errors: ["Non existent order"],
                        statusCode: 404);
                }

                if (order.UserId != userId)
                {
                    logger.LogWarning("[OrderService][GetOrderByIdAsync] User tried to fetch order beloning to different user; requesting id {id}/ actual id {actual}"
                        , userId, order.UserId);
                    return ServiceResult<OrderDto>.Failure(
                        statusCode: 403);
                }

                return ServiceResult<OrderDto>.Success(
                    data: order.ToDto());
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "[OrderService][GetOrderByIdAsync] Unhandled Exception has occured");
                return ServiceResult<OrderDto>.Failure(
                    errors: ["Failed to fetch the order"],
                    statusCode: 500);
            }
        }

        /// <inheritdoc/>
        public async Task<ServiceResult<List<OrderDto>>> GetAllOrdersForUserAsync(Guid userId)
        {
            try
            {
                var user = await userRepo.GetByIdAsync(userId);

                if (user is null)
                {
                    logger.LogWarning("[OrderService][GetAllOrdersForUserAsync] Non existent user attempted to fetch order history");
                    return ServiceResult<List<OrderDto>>.Failure(
                         errors: ["Non existent user"],
                         statusCode: 404);
                }

                var orders = await orderRepo.GetAllForUserAsync(userId);

                return ServiceResult<List<OrderDto>>.Success(
                    data: orders.Select(o => o.ToDto()).ToList());
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "[OrderService][GetAllOrdersForUserAsync] Unhandled Exception has occured");
                return ServiceResult<List<OrderDto>>.Failure(
                    errors: ["Failed to fetch the order history"],
                    statusCode: 500);
            }
        }

        /// <inheritdoc/>
        public async Task<ServiceResult<OrderDto>> CancelOrderAsync(Guid userId, Guid orderId)
        {
            try
            {
                var user = await userRepo.GetByIdAsync(userId);
                if (user is null)
                {
                    logger.LogWarning("[OrderService][CancelOrderAsync] Non existent user attempted to cancel order");
                    return ServiceResult<OrderDto>.Failure(
                         errors: ["Non existent user"],
                         statusCode: 404);
                }

                var order = await orderRepo.GetByIdAsync(orderId);

                if (order is null)
                {
                    logger.LogWarning("[OrderService][CancelOrderAsync] Non existent order cancellation attempt");
                    return ServiceResult<OrderDto>.Failure(
                         errors: ["Non existent order"],
                         statusCode: 404);
                }

                if (order.UserId != userId)
                {
                    logger.LogWarning("[OrderService][CancelOrderAsync] User tried to cancel order belonging to different user; requesting id {id}/ actual id {actual}"
                        , userId, order.UserId);
                    return ServiceResult<OrderDto>.Failure(
                         statusCode: 403);
                }

                if (order.Status != OrderStatus.Submitted)
                {
                    logger.LogWarning("[OrderService][CancelOrderAsync] User tried to cancel in other state than Submited; order id {id}"
                        , order.Id);
                    return ServiceResult<OrderDto>.Failure(
                         errors: ["Only orders in submitted state can be cancelled"],
                         statusCode: 400);
                }

                order.Status = OrderStatus.Cancelled;
                await orderRepo.SaveChangesAsync();

                await auditService.AddNewLogAsync(
                    new AuditLogAddDto
                    {
                        UserId = userId,
                        Action = $"Cancelled order {order.Id}"
                    });

                return ServiceResult<OrderDto>.Success(
                    data: order.ToDto());
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "[OrderService][CancelOrderAsync] Unhandled Exception has occured");
                return ServiceResult<OrderDto>.Failure(
                         errors: ["Failed to cancel the order"],
                         statusCode: 500);
            }
        }

        /// <summary>
        /// Validates products in newly submitted order and retrieves them from the data storage
        /// </summary>
        /// <param name="dto">A <see cref="OrderCreateDto"/> containing list of ordered products and quantities</param>
        /// <returns>A tupple containing:
        /// List of tupples (<see cref="Product"/>, ordered quantities)
        /// and List of encountered errors while validation, or an empty list in case of successful validaton </returns>
        private async Task<(List<(Product, int)> orderedProducts, List<string> errors)> ValidateAndFetchProducts(OrderCreateDto dto)
        {
            var errors = new List<string>();
            var orderedProducts = new List<(Product, int)>();

            foreach (var product in dto.Items)
            {
                var entity = await productRepo.GetByIdAsync(product.ProductId);
                if (entity is null)
                {
                    errors.Add($"Invalid product Id: {product.ProductId}");
                    continue;
                }
                if (entity.StockQuantity < product.Quantity)
                {
                    errors.Add($"Insufficient quantity for product Id: {product.ProductId}");
                    continue;
                }

                orderedProducts.Add((entity, product.Quantity));
            }

            return (orderedProducts, errors);
        }

        /// <summary>
        /// Creates a <see cref="Order"/> model
        /// </summary>
        /// <param name="userId">An <see cref="User"/> ordering the products</param>
        /// <param name="orderedProducts">A list of tupples (<see cref="Product"/>, ordered quantities)</param>
        /// <returns>An <see cref="Order"/> model created based on requested information </returns>
        private async Task<Order> CreateOrder(Guid userId, List<(Product product, int quantity)> orderedProducts)
        {
            var order = await orderRepo.CreateNewOrderAsync(userId);

            var orderItems = new List<OrderItem>();

            foreach (var item in orderedProducts)
            {
                orderItems.Add(new OrderItem
                {
                    OrderId = order.Id,
                    ProductId = item.product.Id,
                    OrderedQuantity = item.quantity,
                    UnitPrice = item.product.Price
                });

                order.OrderPrice += item.product.Price * item.quantity;
                item.product.StockQuantity -= item.quantity;
            }

            orderItems = await orderRepo.AddOrderItemsAsync(orderItems);

            order.Items = orderItems;

            await orderRepo.SaveChangesAsync();

            return order;
        }
    }
}