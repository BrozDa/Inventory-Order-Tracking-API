namespace Inventory_Order_Tracking.API.Services.Interfaces
{
    public interface ICurrentUserService
    {
        public Guid? GetCurentUserId();
    }
}
