using ShippingSystem.Enums;

namespace ShippingSystem.Models
{
    public class ShipmentRequest
    {
        public int ShipmentId { get; set; }
        public Shipment Shipment { get; set; }

        public int RequestId { get; set; }

        public RequestTypeEnum RequestType { get; set; } // Pickup, Return, Exchange ...

        // Navigation للموديل الأساسي
        public RequestBase Request { get; set; }
    }
}
