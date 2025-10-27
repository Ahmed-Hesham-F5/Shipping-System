namespace ShippingSystem.Models
{
    public class ReturnRequest : RequestBase
    {
        public DateOnly ReturnPickupDate { get; set; }
        public TimeOnly ReturnPickupWindowStart { get; set; }
        public TimeOnly ReturnPickupWindowEnd { get; set; }
        public Address ReturnPickupAddress { get; set; } = null!;
        public string CustomerContactName { get; set; } = null!;
        public string CustomerContactPhone { get; set; } = null!;

        public ICollection<ReturnRequestShipment> ReturnRequestShipments { get; set; } = new List<ReturnRequestShipment>();
    }
}
