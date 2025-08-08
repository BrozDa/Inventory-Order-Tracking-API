using System.Net;

namespace Inventory_Order_Tracking.API.Services.Shared
{
    /// <summary>
    /// Represents a standardized result returned from a services.
    /// </summary>
    /// <typeparam name="T">The type of data returned from the operation on success.</typeparam>
    public class ServiceResult<T>
    {
        public bool IsSuccessful { get; set; }
        public HttpStatusCode StatusCode { get; set; }
        public string? ErrorMessage { get; set; }
        public T? Data { get; set; }

        //Documentation is left out from static methods due to self-explanatory nature

        public static ServiceResult<T> Ok(T? data = default)
        {
            return new ServiceResult<T>
            {
                IsSuccessful = true,
                StatusCode = HttpStatusCode.OK,
                Data = data
            };
        }

        public static ServiceResult<T> Created(T data)
        {
            return new ServiceResult<T>
            {
                IsSuccessful = true,
                StatusCode = HttpStatusCode.Created,
                Data = data
            };
        }

        public static ServiceResult<T> NoContent()
        {
            return new ServiceResult<T>
            {
                IsSuccessful = true,
                StatusCode = HttpStatusCode.NoContent,
            };
        }

        public static ServiceResult<T> BadRequest(string? errorMessage)
        {
            return new ServiceResult<T>
            {
                IsSuccessful = false,
                StatusCode = HttpStatusCode.BadRequest,
                ErrorMessage = errorMessage ?? "Bad request"
            };
        }

        public static ServiceResult<T> NotFound(string errorMessage = "Not found")
        {
            return new ServiceResult<T>
            {
                IsSuccessful = false,
                StatusCode = HttpStatusCode.NotFound,
                ErrorMessage = errorMessage
            };
        }

        public static ServiceResult<T> InternalServerError(string? errorMessage)
        {
            return new ServiceResult<T>
            {
                IsSuccessful = false,
                StatusCode = HttpStatusCode.InternalServerError,
                ErrorMessage = errorMessage ?? "Resource not found"
            };
        }

        public static ServiceResult<T> Unauthorized(string errorMessage = "Unauthorized")
        {
            return new ServiceResult<T>
            {
                IsSuccessful = false,
                StatusCode = HttpStatusCode.Unauthorized,
                ErrorMessage = errorMessage
            };
        }

        public static ServiceResult<T> Forbidden()
        {
            return new ServiceResult<T>
            {
                IsSuccessful = false,
                StatusCode = HttpStatusCode.Forbidden,
                ErrorMessage = "Forbidden"
            };
        }
    }
}