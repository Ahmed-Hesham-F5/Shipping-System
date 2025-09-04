using System.ComponentModel.DataAnnotations;

namespace ShippingSystem.DTO
{
    public class ShipmentDetailsDto
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string ShipperId { get; set; } = null!;
        [Required, MaxLength(100)]
        public string ReceiverName { get; set; } = null!;
        [Required, MaxLength(11)]
        public string ReceiverPhone { get; set; } = null!;
        [MaxLength(11)]
        public string? ReceiverAdditionalPhone { get; set; }
        [Required]
        public ReceiverAddressDto ReceiverAddress { get; set; } = null!;
        [Required, MaxLength(255)]
        public string ReceiverEmail { get; set; } = null!;

        [Required, MaxLength(500)]
        public string ShipmentDescription { get; set; } = null!;
        [Required]
        public decimal ShipmentWeight { get; set; }
        [Required]
        public decimal ShipmentLength { get; set; }
        [Required]
        public decimal ShipmentWidth { get; set; }
        [Required]
        public decimal ShipmentHeight { get; set; }
        public decimal ShipmentVolume { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be greater than 0.")]
        public int Quantity { get; set; }
        public string? ShipmentNotes { get; set; }
        public bool CashOnDeliveryEnabled { get; set; }
        public bool OpenPackageOnDeliveryEnabled { get; set; }
        public bool ExpressDeliveryEnabled { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        [Required, MaxLength(30)]
        public string ShipmentTrackingNumber { get; set; } = null!;

        public ICollection<ShipmentStatusDto> ShipmentStatuses { get; set; } = new List<ShipmentStatusDto>();
    }
}
