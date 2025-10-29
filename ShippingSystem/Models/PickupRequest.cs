namespace ShippingSystem.Models
{
    public class PickupRequest : RequestBase
    {
        public DateOnly PickupDate { get; set; }
        public TimeOnly WindowStart { get; set; }
        public TimeOnly WindowEnd { get; set; }
        public Address Address { get; set; } = null!;
        public string ContactName { get; set; } = null!;
        public string ContactPhone { get; set; } = null!;
        public ICollection<PickupRequestShipment> PickupRequestShipments { get; set; } = new List<PickupRequestShipment>();
    }
}