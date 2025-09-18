using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static ShippingSystem.Helpers.DateTimeExtensions;

namespace ShippingSystem.Models
{
    public class ShipmentStatus
    {
        [Key]
        public int Id { get; private set; }
        public int ShipmentId { get; private set; }

        [ForeignKey("ShipmentId")]
        public Shipment Shipment { get; private set; } = null!;

        public string Status { get; private set; } = null!;
        public DateTime Timestamp { get; private set; }
        public string? Notes { get; private set; }

        public ShipmentStatus(int shipmentId, string status, string? notes)
        {
            ShipmentId = shipmentId;
            Status = status;
            Timestamp = UtcNowTrimmedToSeconds();
            Notes = notes;
        }
    }
}
