using Inventory_Order_Tracking.API.Dtos;
using Inventory_Order_Tracking.API.Models;
using Inventory_Order_Tracking.API.Repository.Interfaces;
using Inventory_Order_Tracking.API.Services.Interfaces;
using Inventory_Order_Tracking.API.Services.Shared;

namespace Inventory_Order_Tracking.API.Services
{
    public class OrderService(
        IUserRepository userRepo,
        IProductRepository productRepo,
        IOrderRepository orderRepo,
        ILogger<OrderService> logger) : IOrderService
    {
        public async Task<ServiceResult<OrderDto>> SubmitOrder(Guid userId, CreateOrderDto dto)
        {
            try
            {
                var user = await userRepo.GetByIdAsync(userId);

                if (user is null) //only as double check - user is already authorized in controller level
                {
                    logger.LogWarning("[OrderService][SubmitOrder] Non existent user attempted to submit order");
                    return ServiceResult<OrderDto>.BadRequest("Non existent user");
                }
                var (orderedProducts, errors) = await ValidateAndFetchProducts(dto);

                if (errors.Count > 0)
                {
                    logger.LogWarning("[OrderService][SubmitOrder] Invalid products within an order by {user}, encountered errors:" +
                        "{errors}", userId, errors);

                    return ServiceResult<OrderDto>.BadRequest(string.Join(";", errors));
                }

                var order = await CreateOrder(userId, orderedProducts);

                await orderRepo.SaveChangesAsync(); //this saves even products due to shared context

                var dtoo = order.ToDto();

                return ServiceResult<OrderDto>.Created(order.ToDto());
            }
            catch (Exception ex) 
            {
                logger.LogError(ex, "[OrderService][SubmitOrder] Unhandled Exception has occured");
                return ServiceResult<OrderDto>.InternalServerError("Failed to submit order");
            }
            
        }
        private async Task<(List<(Product, int)> orderedProducts, List<string> errors)> ValidateAndFetchProducts(CreateOrderDto dto)
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

            return(orderedProducts, errors);
        }

        private async Task<Order> CreateOrder(Guid userId, List<(Product product, int quantity)> orderedProducts) 
        {
            var order = await orderRepo.CreateNewOrder(userId);

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

            orderItems = await orderRepo.AddOrderItems(orderItems);

            order.Items = orderItems;

            await orderRepo.SaveChangesAsync();

            return order;
        }
    }
}
