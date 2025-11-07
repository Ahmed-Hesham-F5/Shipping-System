using System.ComponentModel.DataAnnotations.Schema;

namespace ShippingSystem.Models
{
    public class UserAddress
    {
        public int Id { get; set; }
        public string Street { get; set; } = null!;
        public string City { get; set; } = null!;
        public string Governorate { get; set; } = null!;
        public string? Details { get; set; }
        public string? GoogleMapAddressLink { get; set; }

        public string UserID { get; set; } = null!;

        [ForeignKey("UserID")]
        public ApplicationUser User { get; set; } = null!;
    }
}