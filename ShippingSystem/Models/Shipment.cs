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
        
        [Required, MaxLength(100)]
        public string ReceiverName { get; set; } = null!;
        [Required, MaxLength(11)]
        public string ReceiverPhone { get; set; } = null!;

        [Required]
        public ReceiverAddress ReceiverAddress { get; set; } = null!;

        [Required, MaxLength(255)]
        public string ReceiverEmail { get; set; } = null!;

        [Required]
        public decimal Weight { get; set; }
        [Required]
        public decimal Length { get; set; }
        [Required]
        public decimal Width { get; set; }
        [Required]
        public decimal Height { get; set; }

        [NotMapped]
        public decimal Volume => Length * Width * Height;

        public DateTime CreatedAt { get; private set; }
        public DateTime UpdatedAt { get; private set; }

        [Required, MaxLength(30)]
        public string TrackingNumber { get; private set; } = 
            $"SHIP-{DateTime.UtcNow:ddMMyyyy}-{Guid.NewGuid().ToString()[..12]}";
        
        [MaxLength(255)]
        public string? Notes { get; set; }

        public ICollection<ShipmentStatus> ShipmentStatuses { get; set; } = new List<ShipmentStatus>();
    }
}
