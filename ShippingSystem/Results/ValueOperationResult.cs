namespace ShippingSystem.Results
{
    public class ValueOperationResult<T>: OperationResult
    {
        public T? Value { get; set; }
        protected ValueOperationResult(T? value,bool success, int statusCode, string errorMessage) : base(success, statusCode, errorMessage)
        {
            this.Value = value;
        }

        public  static ValueOperationResult<T> Ok(T value) => new ValueOperationResult<T>(value,true, 200, null!);
        public new static ValueOperationResult<T> Fail(int statusCode, string error) => new ValueOperationResult<T>(default,false, statusCode, error);
    }
}
