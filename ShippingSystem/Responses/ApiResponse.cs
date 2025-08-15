namespace ShippingSystem.Response
{
    public class ApiResponse<T>
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public int StatusCode { get; set; }
        public T? Data { get; set; }

        public ApiResponse(bool success, int statusCode, string message)
        {
            Success = success;
            StatusCode = statusCode;
            Message = message;
        }
        public ApiResponse(bool success, int statusCode, string message, T? data = default)
            : this(success, statusCode, message)
        {
            Data = data;
        }
    }
}
