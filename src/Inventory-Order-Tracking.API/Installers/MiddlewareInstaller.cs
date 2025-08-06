namespace Inventory_Order_Tracking.API.Installers
{
    /// <summary>
    /// Profor the <see cref="WebApplication"/>.
    /// </summary>
    public static class MiddlewareInstaller
    {
        /// <summary>
        /// Adds necessary middleware
        /// </summary>
        /// <param name="app">The <see cref="WebApplication"/> instance to extend.</param>
        public static void AddMiddleware(this WebApplication app)
        {
            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseAuthorization();
            app.MapControllers();
        }
    }
}
