using ShippingSystem.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShippingSystem.Models
{
    public class Shipment : ITimeStamped
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string ShipperId { get; set; } = null!;

        [ForeignKey("ShipperId")]
        public Shipper Shipper { get; set; } = null!;

        // Receiver details

        [Required, MaxLength(100)]
        public string ReceiverName { get; set; } = null!;

        [Required, MaxLength(11)]
        public string ReceiverPhone { get; set; } = null!;
       
        [MaxLength(11)]
        public string? ReceiverAdditionalPhone { get; set; }

        [Required]
        public ReceiverAddress ReceiverAddress { get; set; } = null!;

        [Required, MaxLength(255)]
        public string ReceiverEmail { get; set; } = null!;

        // Shipment details

        [Required, MaxLength(500)]
        public string ShipmentDescription { get; set; } = null!;

        [Range(0.01, double.MaxValue, ErrorMessage = "Shipment weight must be greater than 0.")]
        public decimal ShipmentWeight { get; set; }
        [Range(0.01, double.MaxValue, ErrorMessage = "Shipment length must be greater than 0.")]
        public decimal ShipmentLength { get; set; }
        [Range(0.01, double.MaxValue, ErrorMessage = "Shipment width must be greater than 0.")]
        public decimal ShipmentWidth { get; set; }
        [Range(0.01, double.MaxValue, ErrorMessage = "Shipment height must be greater than 0.")]
        public decimal ShipmentHeight { get; set; }

        [NotMapped]
        public decimal ShipmentVolume => ShipmentLength * ShipmentWidth * ShipmentHeight;

        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be greater than 0.")]
        public int Quantity { get; set; }

        [MaxLength(500)]
        public string? ShipmentNotes { get; set; }
        public ICollection<ShipmentStatus> ShipmentStatuses { get; set; } = new List<ShipmentStatus>();

        // Delivery options
        public bool CashOnDeliveryEnabled { get; set; }
        public bool OpenPackageOnDeliveryEnabled { get; set; }
        public bool ExpressDeliveryEnabled { get; set; }

        // Auto-managed properties
        public DateTime CreatedAt { get; private set; }
        public DateTime UpdatedAt { get; private set; }
        public string ShipmentTrackingNumber { get; private set; } = 
            $"SHIP-{DateTime.UtcNow:ddMMyyyy}-{Guid.NewGuid().ToString("N")[..12]}";
    }
}
