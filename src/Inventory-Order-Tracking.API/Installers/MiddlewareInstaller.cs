using System.Runtime.CompilerServices;

namespace Inventory_Order_Tracking.API.Installers
{
    public static class MiddlewareInstaller
    {
        public static void AddMiddleware(this WebApplication app)
        {
            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseAuthorization();
            app.MapControllers();
        }
    }
}
