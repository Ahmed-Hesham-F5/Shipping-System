using ShippingSystem.DTOs.AddressDTOs;
using System.ComponentModel.DataAnnotations;

namespace ShippingSystem.DTOs.RequestDTOs
{
    public class CreateExchangeRequestDto
    {
        [Required]
        public AddressDto PickupAddress { get; set; } = null!;
        [Required]
        public string CustomerName { get; set; } = null!;
        [Required]
        [MaxLength(11, ErrorMessage = "Phone number must be 11 digits.")]
        [RegularExpression(@"^(010|011|012|015)\d{8}$", ErrorMessage = "Phone number must start with 010, 011, 012 or 015.")]
        public string CustomerPhone { get; set; } = null!;
        [Required]
        public AddressDto CustomerAddress { get; set; } = null!;
        public string? ExchangeReason { get; set; }

        public List<int> ToCustomer { get; set; } = new();
        public List<int> FromCustomer { get; set; } = new();
    }
}
