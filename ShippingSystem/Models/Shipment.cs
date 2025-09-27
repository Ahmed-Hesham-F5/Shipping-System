using System.ComponentModel.DataAnnotations.Schema;

namespace ShippingSystem.Models
{
    public class Shipment
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
        public int Quantity { get; set; }
        public string? ShipmentNotes { get; set; }
        public ICollection<ShipmentStatus> ShipmentStatuses { get; set; } = new List<ShipmentStatus>();

        // Delivery options
        public bool CashOnDeliveryEnabled { get; set; }
        public bool OpenPackageOnDeliveryEnabled { get; set; }
        public bool ExpressDeliveryEnabled { get; set; }
        public decimal CollectionAmount { get; set; }

        // Will be managed by Hub
        public decimal AdditionalWeight { get; set; }

        // Will be managed by the Routing Algorithm
        public decimal ShippingCost { get; set; } = 20m; // Default shipping cost (will be changed later)

        // Managed by Shipping Setting (Hub/Admin detemines the shipping setting values)
        public decimal AdditionalWeightCostPrtKg { get; set; }
        public decimal CollectionFeePercentage { get; set; }
        public decimal CollectionFeeThreshold { get; set; }

        // Auto-managed properties
        [NotMapped]
        public decimal ShipmentVolume => ShipmentLength * ShipmentWidth * ShipmentHeight;
        [NotMapped]
        public decimal CollectionFee => CollectionAmount > CollectionFeeThreshold ? CollectionAmount * CollectionFeePercentage : 0m;
        [NotMapped]
        public decimal AdditionalWeightCost => AdditionalWeight * AdditionalWeightCostPrtKg;
        [NotMapped]
        public decimal AdditionalCost => AdditionalWeightCost + CollectionFee;
        [NotMapped]
        public decimal TotalCost => ShippingCost + AdditionalCost;
        [NotMapped]
        public decimal NetPayout => CollectionAmount - TotalCost;

        // Managed when adding the shipment
        public string ShipmentTrackingNumber { get; set; } = null!;

        // Timestamps, Managed in Add/Update operations
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
