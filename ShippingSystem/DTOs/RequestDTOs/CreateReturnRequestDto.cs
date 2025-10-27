using ShippingSystem.DTOs.AddressDTOs;
using System.ComponentModel.DataAnnotations;

namespace ShippingSystem.DTOs.RequestDTOs
{
    public class CreateReturnRequestDto
    {
        [Required]
        public DateOnly ReturnPickupDate { get; set; }
        [Required]
        public TimeOnly ReturnPickupWindowStart { get; set; }
        [Required]
        public TimeOnly ReturnPickupWindowEnd { get; set; }
        [Required]
        public AddressDto ReturnPickupAddress { get; set; } = null!;
        [Required, MaxLength(100)]
        public string CustomerContactName { get; set; } = null!;
        [Required]
        [MaxLength(11, ErrorMessage = "Phone number must be 11 digits.")]
        [RegularExpression(@"^(010|011|012|015)\d{8}$", ErrorMessage = "Phone number must start with 010, 011, 012 or 015.")]
        public string CustomerContactPhone { get; set; } = null!;

        public List<int> ShipmentIds { get; set; } = new List<int>();
    }
}
