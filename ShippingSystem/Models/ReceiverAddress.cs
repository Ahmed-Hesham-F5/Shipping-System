using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace ShippingSystem.Models
{
    [Owned]
    public class ReceiverAddress
    {
        [Required, MaxLength(100)]
        public string Street { get; set; } = null!;

        [Required, MaxLength(50)]
        public string City { get; set; } = null!;

        [MaxLength(50)]
        public string Country { get; set; } = "Egypt";

        [MaxLength(500)]
        public string? Details { get; set; }

        [MaxLength(255), Url]
        public string? GoogleMapAddressLink { get; set; }
    }
}
