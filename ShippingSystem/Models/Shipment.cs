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
        public decimal CollectionAmount { get; set; } = 0m; // Default collection amount if cod is disabled

        // Managed by Hub/Admin
        public decimal ShippingCost { get; set; } = 20m; // Default shipping cost (will be changed later)
        public decimal AdditionalWeight { get; set; } = 0m; // Weight above the base weight
        public decimal AdditionalWeightCostPrtKg { get; set; } = 5m; // Default additional weight cost per kg
        public decimal CollectionFeePercentage { get; set; } = 0.01m; // Default 1% fee
        public decimal CollectionFeeThreshold { get; set; } = 3000m; // Default minimum for fee application

        [NotMapped]
        public decimal CollectionFee => CollectionAmount > CollectionFeeThreshold ? CollectionAmount * CollectionFeePercentage : 0m;
        [NotMapped]
        public decimal AdditionalWeightCost => AdditionalWeight * AdditionalWeightCostPrtKg;
        [NotMapped]
        public decimal AdditionalCost => AdditionalWeightCost + CollectionFee;

        // Auto-managed properties
        public DateTime CreatedAt { get; private set; }
        public DateTime UpdatedAt { get; private set; }
        public string ShipmentTrackingNumber { get; private set; } =
            $"SHIP-{DateTime.UtcNow:ddMMyyyy}-{Guid.NewGuid().ToString("N")[..12]}";
    }
}
