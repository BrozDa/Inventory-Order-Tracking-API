using System.Net;

namespace Inventory_Order_Tracking.API.Services.Shared
{
    public class EmailVerificationServiceResult
    {
        public required bool IsSuccessful { get; set; }
        public required HttpStatusCode StatusCode { get; set; }
        public string? ErrorMessage { get; set; }

        public static EmailVerificationServiceResult Ok()
        {
            return new EmailVerificationServiceResult
            {
                IsSuccessful = true,
                StatusCode = HttpStatusCode.OK,
            };
        }
        public static EmailVerificationServiceResult Unauthorized(string? errorMessage = "Unauthorized")
        {
            return new EmailVerificationServiceResult
            {
                IsSuccessful = false,
                StatusCode = HttpStatusCode.Unauthorized,
                ErrorMessage = errorMessage
            };
        }
        public static EmailVerificationServiceResult InternalServerError(string? errorMessage = "Internal Server Error")
        {
            return new EmailVerificationServiceResult
            {
                IsSuccessful = false,
                StatusCode = HttpStatusCode.InternalServerError,
                ErrorMessage = errorMessage
            };
        }
    }
}
