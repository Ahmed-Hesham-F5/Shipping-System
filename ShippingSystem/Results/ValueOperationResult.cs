using System;

namespace ShippingSystem.Results
{
    public class ValueOperationResult<T>: OperationResult
    {
        public T? Value { get; set; }
        protected ValueOperationResult(T? value,bool success, string errorMessage) : base(success, errorMessage)
        {
            this.Value = value;
        }

        public static ValueOperationResult<T> Ok(T value) => new ValueOperationResult<T>(value,true, null);
        public new static ValueOperationResult<T> Fail(string error) => new ValueOperationResult<T>(default,false, error);


    }
}
