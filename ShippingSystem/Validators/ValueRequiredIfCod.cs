using System.ComponentModel.DataAnnotations;

namespace ShippingSystem.Validators
{
    public class ValueRequiredIfCod : ValidationAttribute
    {
        private readonly string _codPropertyName;
        public ValueRequiredIfCod(string codPropertyName)
        {
            _codPropertyName = codPropertyName;
        }

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            var codProperty = validationContext.ObjectType.GetProperty(_codPropertyName);

            if (codProperty == null)
                return new ValidationResult($"Unknown property: {_codPropertyName}");

            var codValue = (bool)codProperty.GetValue(validationContext.ObjectInstance)!;

            if (codValue)
            {
                var amount = value as decimal?;

                if (amount == null || amount <= 0)
                {
                    return new ValidationResult("CollectionAmount is required when CashOnDeliveryEnabled is true and must be greater than 0.");
                }
            }

            return ValidationResult.Success;
        }
    }
}
