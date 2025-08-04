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
        IOrderRepository orderRepo) : IOrderService
    {
        public async Task<ServiceResult<OrderDto>> SubmitOrder(Guid userId, CreateOrderDto dto)
        {
            var user = await userRepo.GetByIdAsync(userId);
            if(user is null)
                return ServiceResult<OrderDto>.BadRequest("Non existent user");


            var (orderedProducts, errors) = await ValidateAndFetchProducts(dto);

            if (errors.Count > 0)
                return ServiceResult<OrderDto>.BadRequest(string.Join(";", errors));


            var order = await CreateOrder(userId, orderedProducts);

            await orderRepo.SaveChangesAsync(); //this saves even products due to shared context


            return ServiceResult<OrderDto>.Ok(order.ToDto());
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
