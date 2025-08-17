using ShippingSystem.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShippingSystem.DTO
{
    public class GetShipmentsDto
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string ShipperId { get; set; } = null!;
        [Required, MaxLength(100)]
        public string ReceiverName { get; set; } = null!;
        [Required, MaxLength(11)]
        public string ReceiverPhone { get; set; } = null!;

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

        [NotMapped]
        public decimal ShipmentVolume { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        [Required, MaxLength(30)]
        public string ShipmentTrackingNumber { get; set; } = null!;

        public ICollection<ShipmentStatusDto> ShipmentStatuses { get; set; } = new List<ShipmentStatusDto>();
    }
}
