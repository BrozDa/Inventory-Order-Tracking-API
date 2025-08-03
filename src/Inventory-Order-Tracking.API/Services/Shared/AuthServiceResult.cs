using System.Net;

namespace Inventory_Order_Tracking.API.Services.Shared
{
    public class AuthServiceResult<T>
    {
        public bool IsSuccessful { get; set; }
        public HttpStatusCode StatusCode { get; set; }
        public string? ErrorMessage { get; set; }
        public T? Data { get; set; }

        public static AuthServiceResult<T> Ok(T? data = default)
        {
            return new AuthServiceResult<T>
            {
                IsSuccessful = true,
                StatusCode = HttpStatusCode.OK,
                Data = data
            };
        }

        public static AuthServiceResult<T> BadRequest(string? errorMessage)
        {
            return new AuthServiceResult<T>
            {
                IsSuccessful = false,
                StatusCode = HttpStatusCode.BadRequest,
                ErrorMessage = errorMessage ?? "Bad request"
            };
        }

        public static AuthServiceResult<T> InternalServerError(string? errorMessage)
        {
            return new AuthServiceResult<T>
            {
                IsSuccessful = false,
                StatusCode = HttpStatusCode.InternalServerError,
                ErrorMessage = errorMessage ?? "Resource not found"
            };
        }

        public static AuthServiceResult<T> Unauthorized()
        {
            return new AuthServiceResult<T>
            {
                IsSuccessful = false,
                StatusCode = HttpStatusCode.Unauthorized,
                ErrorMessage = "Unauthorized"
            };
        }
    }
}