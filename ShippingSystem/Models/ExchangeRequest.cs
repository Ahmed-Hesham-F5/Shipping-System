namespace ShippingSystem.Models
{
    public class ExchangeRequest : RequestBase
    {
        public Address PickupAddress { get; set; } = null!;
        public string ShipperName { get; set; } = null!;
        public string ShipperPhone { get; set; } = null!;
        public Address CustomerAddress { get; set; } = null!;
        public string CustomerName { get; set; } = null!;
        public string CustomerPhone { get; set; } = null!;
        public string? CustomerEmail { get; set; } = null!;
        public string? ExchangeReason { get; set; }

        public List<ExchangeRequestShipment> ExchangeRequestShipments { get; set; } = new();
    }
}
