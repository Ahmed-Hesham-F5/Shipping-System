using System.ComponentModel.DataAnnotations.Schema;

namespace ShippingSystem.Models
{
    public class ReturnRequestShipment
    {
        public int ReturnRequestId { get; set; }
        [ForeignKey("ReturnRequestId")]
        public ReturnRequest ReturnRequest { get; set; } = null!;
        public int ShipmentId { get; set; }
        [ForeignKey("ShipmentId")]
        public Shipment Shipment { get; set; } = null!;
    }
}
