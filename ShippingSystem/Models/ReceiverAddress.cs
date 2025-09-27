using Microsoft.EntityFrameworkCore;

namespace ShippingSystem.Models
{
    [Owned]
    public class ReceiverAddress
    {
        public string Street { get; set; } = null!;
        public string City { get; set; } = null!;
        public string Governorate { get; set; } = null!;
        public string? Details { get; set; }
        public string? GoogleMapAddressLink { get; set; }
    }
}
