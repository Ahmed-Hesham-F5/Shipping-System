namespace ShippingSystem.Models
{
    public class ReturnRequest : RequestBase
    {
        public Address CustomerAddress { get; set; } = null!;
        public string CustomerName { get; set; } = null!;
        public string CustomerPhone { get; set; } = null!;
        public string? CustomerEmail { get; set; } = null!;

        public ICollection<ReturnRequestShipment> ReturnRequestShipments { get; set; } = new List<ReturnRequestShipment>();
    }
}
