namespace Inventory_Order_Tracking.API.Services.Interfaces
{
    /// <summary>
    /// Defines a contract for retrieving information about the currently authenticated user.
    /// </summary>
    public interface ICurrentUserService
    {
        /// <summary>
        /// Retrieves the Id of the currently authenticated user from the claims.
        /// </summary>
        /// <returns>
        /// A <see cref="Guid"/> representing the user's ID if available and valid; otherwise,null.
        /// </returns>
        Guid? GetCurentUserId();
    }
}
