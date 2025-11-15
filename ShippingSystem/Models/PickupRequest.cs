namespace ShippingSystem.Models
{
    public class PickupRequest : RequestBase
    {
        public Address PickupAddress { get; set; } = null!;
        public ICollection<PickupRequestShipment> PickupRequestShipments { get; set; } = new List<PickupRequestShipment>();
    }
}