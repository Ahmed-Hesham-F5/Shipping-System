using System.ComponentModel.DataAnnotations;

namespace ShippingSystem.DTOs
{
    public class CreateReturnRequestDto
    {
        [Required]
        public DateOnly ReturnPickupDate { get; set; }
        [Required]
        public TimeOnly ReturnPickupWindowStart { get; set; }
        [Required]
        public TimeOnly ReturnPickupWindowEnd { get; set; }
        [Required, MaxLength(256)]
        public string CustomerStreet { get; set; } = null!;
        [Required, MaxLength(50)]
        public string CustomerCity { get; set; } = null!;
        [Required, MaxLength(50)]
        public string CustomerGovernorate { get; set; } = null!;
        [MaxLength(500)]
        public string? CustomerAddressDetails { get; set; }
        [MaxLength(2083), Url]
        public string? CustomerGoogleMapAddressLink { get; set; }
        [Required, MaxLength(100)]
        public string CustomerContactName { get; set; } = null!;
        [Required]
        [MaxLength(11, ErrorMessage = "Phone number must be 11 digits.")]
        [RegularExpression(@"^(010|011|012|015)\d{8}$", ErrorMessage = "Phone number must start with 010, 011, 012 or 015.")]
        public string CustomerContactPhone { get; set; } = null!;
        
        [Required]
        public DateOnly ReturnDate { get; set; }
        [Required]
        public TimeOnly ReturnWindowStart { get; set; }
        [Required]
        public TimeOnly ReturnWindowEnd { get; set; }
        [Required, MaxLength(256)]
        public string ShipperStreet { get; set; } = null!;
        [Required, MaxLength(50)]
        public string ShipperCity { get; set; } = null!;
        [Required, MaxLength(50)]
        public string ShipperGovernorate { get; set; } = null!;
        [MaxLength(500)]
        public string? ShipperAddressDetails { get; set; }
        [MaxLength(2083), Url]
        public string? ShipperGoogleMapAddressLink { get; set; }
        [Required, MaxLength(100)]
        public string ShipperContactName { get; set; } = null!;
        [Required]
        [MaxLength(11, ErrorMessage = "Phone number must be 11 digits.")]
        [RegularExpression(@"^(010|011|012|015)\d{8}$", ErrorMessage = "Phone number must start with 010, 011, 012 or 015.")]
        public string ShipperContactPhone { get; set; } = null!;

        public List<int> ShipmentIds { get; set; } = new List<int>();
    }
}
