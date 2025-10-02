using System.ComponentModel.DataAnnotations;

namespace ShippingSystem.DTOs
{
    public class CreatePickupRequestDto
    {
        [Required]
        public DateOnly PickupDate { get; set; }
        [Required]
        public TimeOnly WindowStart { get; set; }
        [Required]
        public TimeOnly WindowEnd { get; set; }
        [Required, MaxLength(256)]
        public string Street { get; set; } = null!;
        [Required, MaxLength(50)]
        public string City { get; set; } = null!;
        [Required, MaxLength(50)]
        public string Governorate { get; set; } = null!;
        [MaxLength(500)]
        public string? Details { get; set; }
        [Required, MaxLength(100)]
        public string ContactName { get; set; } = null!;
        [Required]
        [MaxLength(11, ErrorMessage = "Phone number must be 11 digits.")]
        [RegularExpression(@"^(010|011|012|015)\d{8}$", ErrorMessage = "Phone number must start with 010, 011, 012 or 015.")]
        public string ContactPhone { get; set; } = null!;
        public List<int> ShipmentIds { get; set; } = new List<int>();
    }
}
