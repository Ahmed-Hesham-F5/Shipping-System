using System.ComponentModel.DataAnnotations;

namespace ShippingSystem.DTOs.PhoneNumberDTOs
{
    public class PhoneNumberDto
    {
        [Required]
        [MaxLength(11, ErrorMessage = "Phone number must be 11 digits.")]
        [RegularExpression(@"^(010|011|012|015)\d{8}$", ErrorMessage = "Phone number must start with 010, 011, 012 or 015.")]
        public string PhoneNumber { get; set; } = null!;
    }
}
