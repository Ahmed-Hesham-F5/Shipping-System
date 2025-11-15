using ShippingSystem.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShippingSystem.Models
{
    public class ExchangeRequestShipment 
    {
        [ForeignKey("ExchangeRequest")]
        public int ExchangeRequestId { get; set; }
        public ExchangeRequest ExchangeRequest { get; set; } = null!;
        [ForeignKey("Shipment")]
        public int ShipmentId { get; set; }
        public Shipment Shipment { get; set; } = null!;
        
        public ExchangeDirectionEnum ExchangeDirection { get; set; }
    }
}
