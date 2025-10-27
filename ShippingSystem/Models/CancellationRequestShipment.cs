using System.ComponentModel.DataAnnotations.Schema;

namespace ShippingSystem.Models
{
    public class CancellationRequestShipment
    {
        public int CancellationRequestId { get; set; }
        [ForeignKey("CancellationRequestId")]
        public CancellationRequest CancellationRequest { get; set; } = null!;
        public int ShipmentId { get; set; }
        [ForeignKey("ShipmentId")]
        public Shipment Shipment { get; set; } = null!;
    }
}
