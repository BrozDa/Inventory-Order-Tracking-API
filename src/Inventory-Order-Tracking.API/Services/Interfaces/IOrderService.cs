using Inventory_Order_Tracking.API.Dtos;
using Inventory_Order_Tracking.API.Models;
using Inventory_Order_Tracking.API.Services.Shared;

namespace Inventory_Order_Tracking.API.Services.Interfaces
{
    public interface IOrderService
    {
        Task<ServiceResult<OrderDto>> SubmitOrderAsync(Guid userId, CreateOrderDto dto);
        Task<ServiceResult<OrderDto>> GetOrderByIdAsync(Guid userId, Guid orderId);
        Task<ServiceResult<List<OrderDto>>> GetAllOrdersForUserAsync(Guid userId);
        Task<ServiceResult<OrderDto>> CancelOrderAsync(Guid userId, Guid orderId);
    }
}
