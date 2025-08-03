using System.Net;

namespace Inventory_Order_Tracking.API.Services.Shared
{
    public class ProductServiceResult<T>
    {
        public required bool IsSuccessful { get; set; }
        public required HttpStatusCode StatusCode { get; set; }
        public string? ErrorMessage { get; set; }
        public T? Data { get; set; }

        public static ProductServiceResult<T> Ok(T? data = default)
        {
            return new ProductServiceResult<T>
            {
                IsSuccessful = true,
                StatusCode = HttpStatusCode.OK,
                Data = data
            };
        }
        public static ProductServiceResult<T> Created(T data)
        {
            return new ProductServiceResult<T>
            {
                IsSuccessful = true,
                StatusCode = HttpStatusCode.Created,
                Data = data
            };
        }
        public static ProductServiceResult<T> NoContent()
        {
            return new ProductServiceResult<T>
            {
                IsSuccessful = true, 
                StatusCode = HttpStatusCode.NoContent,
            };
        }
        public static ProductServiceResult<T> BadRequest(string? errorMessage)
        {
            return new ProductServiceResult<T>
            {
                IsSuccessful = false,
                StatusCode = HttpStatusCode.BadRequest,
                ErrorMessage = errorMessage ?? "Bad request"
            };
        }
        public static ProductServiceResult<T> NotFound(string? errorMessage = "Not found")
        {
            return new ProductServiceResult<T>
            {
                IsSuccessful = false,
                StatusCode = HttpStatusCode.NotFound,
                ErrorMessage = errorMessage ?? "Bad request"
            };
        }
        public static ProductServiceResult<T> InternalServerError(string? errorMessage)
        {
            return new ProductServiceResult<T>
            {
                IsSuccessful = false,
                StatusCode = HttpStatusCode.InternalServerError,
                ErrorMessage = errorMessage ?? "Resource not found"
            };
        }
        public static ProductServiceResult<T> Unauthorized()
        {
            return new ProductServiceResult<T>
            {
                IsSuccessful = false,
                StatusCode = HttpStatusCode.Unauthorized,
                ErrorMessage = "Unauthorized"
            };

        }
    }

}
