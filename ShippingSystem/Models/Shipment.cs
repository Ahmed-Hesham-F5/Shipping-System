using ShippingSystem.Interfaces;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShippingSystem.Models
{
    public class Shipment : ITimeStamped
    {
        public int Id { get; set; }
        public string ShipperId { get; set; } = null!;

        [ForeignKey("ShipperId")]
        public Shipper Shipper { get; set; } = null!;

        // Receiver details
        public string ReceiverName { get; set; } = null!;
        public string ReceiverPhone { get; set; } = null!;
        public string? ReceiverAdditionalPhone { get; set; }
        public ReceiverAddress ReceiverAddress { get; set; } = null!;
        public string ReceiverEmail { get; set; } = null!;

        // Shipment details
        public string ShipmentDescription { get; set; } = null!;
        public decimal ShipmentWeight { get; set; }
        public decimal ShipmentLength { get; set; }
        public decimal ShipmentWidth { get; set; }
        public decimal ShipmentHeight { get; set; }
        [NotMapped]
        public decimal ShipmentVolume => ShipmentLength * ShipmentWidth * ShipmentHeight;
        public int Quantity { get; set; }
        public string? ShipmentNotes { get; set; }
        public ICollection<ShipmentStatus> ShipmentStatuses { get; set; } = new List<ShipmentStatus>();

        // Delivery options
        public bool CashOnDeliveryEnabled { get; set; }
        public bool OpenPackageOnDeliveryEnabled { get; set; }
        public bool ExpressDeliveryEnabled { get; set; }
        public decimal? CollectionAmount { get; set; }

        // Managed by Hub/Admin
        public decimal? ShippingCost { get; set; }
        public decimal? AdditionalWeight { get; set; }
        public decimal? AdditionalWeightCost { get; set; }
        public decimal? CollectionFee { get; set; }
        public decimal? AdditionalCost => (AdditionalWeight * AdditionalWeightCost) + CollectionFee;

        // Auto-managed properties
        public DateTime CreatedAt { get; private set; }
        public DateTime UpdatedAt { get; private set; }
        public string ShipmentTrackingNumber { get; private set; } = 
            $"SHIP-{DateTime.UtcNow:ddMMyyyy}-{Guid.NewGuid().ToString("N")[..12]}";
    }
}
