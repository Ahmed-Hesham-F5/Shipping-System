using ShippingSystem.DTOs.AddressDTOs;
using System.ComponentModel.DataAnnotations;

namespace ShippingSystem.DTOs.HubDTOs
{
    public class CreateHubDto
    {
        [Required, MaxLength(50)]
        public string Type { get; set; } = null!;
        [Required, MaxLength(100)]
        public string Name { get; set; } = null!;
        [Required]
        public AddressDto Address { get; set; } = null!;
        [Required]
        [MaxLength(11, ErrorMessage = "Phone number must be 11 digits.")]
        [RegularExpression(@"^(010|011|012|015)\d{8}$", ErrorMessage = "Phone number must start with 010, 011, 012 or 015.")]
        public string PhoneNumber { get; set; } = null!;
        [Required, Range(1, int.MaxValue, ErrorMessage = "Area must be greater than 0.")]
        public decimal AreaInSquareMeters { get; set; }
    }
}
