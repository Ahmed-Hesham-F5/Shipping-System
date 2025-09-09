namespace ShippingSystem.Results
{
    public class OperationResult
    {
        public bool Success { get; set; }
        public int StatusCode { get; set; }
        public string ErrorMessage { get; set; }

        protected OperationResult(bool success, int statusCode, string errorMessage)
        {
            Success = success;
            StatusCode = statusCode;
            ErrorMessage = errorMessage;
        }
        public static OperationResult Ok() => new OperationResult(true, 200, null!);
        public static OperationResult Fail(int statusCode, string error) => new OperationResult(false, statusCode, error);
    }
}