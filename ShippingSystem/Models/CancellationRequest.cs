namespace ShippingSystem.Models
{
    public class CancellationRequest : RequestBase
    {
        public ICollection<CancellationRequestShipment> CancellationRequestShipments { get; set; } = new List<CancellationRequestShipment>();
    }
}
