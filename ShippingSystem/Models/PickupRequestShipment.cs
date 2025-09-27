using System.ComponentModel.DataAnnotations.Schema;

namespace ShippingSystem.Models
{
    public class PickupRequestShipment
    {
        public int PickupRequestId { get; set; }
        [ForeignKey("PickupRequestId")]
        public PickupRequest PickupRequest { get; set; } = null!;
        public int ShipmentId { get; set; }
        [ForeignKey("ShipmentId")]
        public Shipment Shipment { get; set; } = null!;
    }
}
