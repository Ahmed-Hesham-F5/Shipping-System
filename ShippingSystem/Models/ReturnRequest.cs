namespace ShippingSystem.Models
{
    public class ReturnRequest : RequestBase
    {
        public DateOnly PickupDate { get; set; }
        public TimeOnly WindowStart { get; set; }
        public TimeOnly WindowEnd { get; set; }
        public Address CustomerAddress { get; set; } = null!;
        public string CustomerContactName { get; set; } = null!;
        public string CustomerContactPhone { get; set; } = null!;
        public string? CustomerEmail { get; set; } = null!;

        public ICollection<ReturnRequestShipment> ReturnRequestShipments { get; set; } = new List<ReturnRequestShipment>();
    }
}
