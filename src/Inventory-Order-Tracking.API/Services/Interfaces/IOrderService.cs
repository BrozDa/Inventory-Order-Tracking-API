using Inventory_Order_Tracking.API.Dtos;
using Inventory_Order_Tracking.API.Services.Shared;

namespace Inventory_Order_Tracking.API.Services.Interfaces
{
    public interface IOrderService
    {
        Task<ServiceResult<OrderDto>> SubmitOrder(Guid userId, CreateOrderDto dto);
        Task<ServiceResult<OrderDto>> GetOrderById(Guid userId, Guid orderId);
    }
}
