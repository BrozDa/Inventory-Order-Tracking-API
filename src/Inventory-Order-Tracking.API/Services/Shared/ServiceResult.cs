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
        public int StatusCode { get; set; }
        public string? Message { get; set; }
        public List<string>? Errors { get; set; }
        public T? Data { get; set; }


        public static ServiceResult<T> Success(T? data = default, string? message = null, int statusCode = 200)
        {
            return new ServiceResult<T>
            {
                IsSuccessful = true,
                StatusCode = statusCode,
                Message = message,
                Data = data
            };
        }
        public static ServiceResult<T> Failure(T? data = default, List<string>? errors = null, int statusCode = 400)
        {
            return new ServiceResult<T>
            {
                IsSuccessful = true,
                StatusCode = statusCode,
                Errors = errors,
                Data = data
            };
        }
    }
}