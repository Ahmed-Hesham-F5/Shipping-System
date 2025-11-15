using ShippingSystem.DTOs.AddressDTOs;
using System.ComponentModel.DataAnnotations;

namespace ShippingSystem.DTOs.RequestDTOs
{
    public class CreateReturnRequestDto
    {
        [Required]
        public AddressDto ShipperAddress { get; set; } = null!;
        [Required]
        public AddressDto CustomerAddress { get; set; } = null!;
        [Required, MaxLength(100)]
        public string CustomerName { get; set; } = null!;
        [Required]
        [MaxLength(11, ErrorMessage = "Phone number must be 11 digits.")]
        [RegularExpression(@"^(010|011|012|015)\d{8}$", ErrorMessage = "Phone number must start with 010, 011, 012 or 015.")]
        public string CustomerPhone { get; set; } = null!;
        [MaxLength(255), EmailAddress]
        public string? CustomerEmail { get; set; } = null!;

        public List<int> ShipmentIds { get; set; } = new List<int>();
    }
}
