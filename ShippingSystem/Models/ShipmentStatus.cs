using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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

        // Factory method to enforce immutability
        public static ShipmentStatus Create(int shipmentId, string status, string? notes)
        {
            return new ShipmentStatus
            {
                ShipmentId = shipmentId,
                Status = status,
                Timestamp = DateTime.UtcNow,
                Notes = notes
            };
        }
    }
}
