using System.ComponentModel.DataAnnotations;

namespace ShippingSystem.DTOs.AddressDTOs
{
    public class AddressDto
    {
        [Required, MaxLength(256)]
        public string Street { get; set; } = null!;
        [Required, MaxLength(50)]
        public string City { get; set; } = null!;
        [Required, MaxLength(50)]
        public string Governorate { get; set; } = null!;
        [MaxLength(500)]
        public string? Details { get; set; }
        [MaxLength(2083), Url]
        public string? GoogleMapAddressLink { get; set; }
    }
}
