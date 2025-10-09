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

        public DateOnly ReturnDate { get; set; }
        public TimeOnly ReturnWindowStart { get; set; }
        public TimeOnly ReturnWindowEnd { get; set; }
        public Address ReturnAddress { get; set; } = null!;
        public string ShipperContactName { get; set; } = null!;
        public string ShipperContactPhone { get; set; } = null!;

        public ICollection<ReturnRequestShipment> ReturnRequestShipments { get; set; } = new List<ReturnRequestShipment>();
    }
}
