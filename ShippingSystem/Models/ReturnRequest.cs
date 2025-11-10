namespace ShippingSystem.Models
{
    public class ReturnRequest : RequestBase
    {
        public Address CustomerAddress { get; set; } = null!;
        public string CustomerContactName { get; set; } = null!;
        public string CustomerContactPhone { get; set; } = null!;
        public string? CustomerEmail { get; set; } = null!;

        public ICollection<ReturnRequestShipment> ReturnRequestShipments { get; set; } = new List<ReturnRequestShipment>();
    }
}
