using System.ComponentModel.DataAnnotations.Schema;

namespace ShippingSystem.Models
{
    public class PickupRequest
    {
        public int Id { get; set; }
        public string ShipperId { get; set; } = null!;
        [ForeignKey("ShipperId")]
        public ApplicationUser Shipper { get; set; } = null!;
        public DateOnly PickupDate { get; set; }
        public TimeOnly WindowStart { get; set; }
        public TimeOnly WindowEnd { get; set; }
        public string Street { get; set; } = null!;
        public string City { get; set; } = null!;
        public string Governorate { get; set; } = null!;
        public string? Details { get; set; }
        public string ContactName { get; set; } = null!;
        public string ContactPhone { get; set; } = null!;
        public ICollection<PickupRequestShipment> PickupRequestShipments { get; set; } = new List<PickupRequestShipment>();
    }
}
