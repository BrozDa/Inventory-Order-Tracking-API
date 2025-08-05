namespace Inventory_Order_Tracking.API.Services.Interfaces
{
    public interface ICurrentUserService
    {
        Guid? GetCurentUserId();
    }
}
