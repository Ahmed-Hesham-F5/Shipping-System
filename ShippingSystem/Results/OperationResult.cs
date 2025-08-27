namespace ShippingSystem.Results
{
    public class OperationResult
    {
        public bool Success { get; set; }
        public string ErrorMessage { get; set; }


        protected OperationResult(bool success, string errorMessage)
        {
            Success = success;
            ErrorMessage = errorMessage;
        }
        public static OperationResult Ok() => new OperationResult( true, null);
        public static OperationResult Fail(string error) => new OperationResult( false, error);

    }
}