namespace ShippingSystem.Models
{
    public class PickupRequest : RequestBase
    {
        public Address PickupAddress { get; set; } = null!;
        public string ContactName { get; set; } = null!;
        public string ContactPhone { get; set; } = null!;
        public ICollection<PickupRequestShipment> PickupRequestShipments { get; set; } = new List<PickupRequestShipment>();
    }
}