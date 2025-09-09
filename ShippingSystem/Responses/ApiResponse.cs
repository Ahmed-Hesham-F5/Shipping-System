namespace ShippingSystem.Responses
{
    public class ApiResponse<T>
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public T? Data { get; set; }

        public ApiResponse(bool success, string message)
        {
            Success = success;
            Message = message;
        }

        public ApiResponse(bool success, string message, T? data = default)
            : this(success, message)
        {
            Data = data;
        }
    }
}
