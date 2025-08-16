using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShippingSystem.Models
{
    public class ShipperAddress
    {
        [Key]
        public int Id { get; set; }

        [Required, MaxLength(100)]
        public string Street { get; set; } = null!;

        [Required, MaxLength(50)]
        public string City { get; set; } = null!;

        [MaxLength(50)]
        public string Country { get; set; } = "Egypt";

        [MaxLength(500)]
        public string? Details { get; set; }

        [Required]
        public string ShipperId { get; set; } = null!;

        [ForeignKey("ShipperId")]
        public Shipper Shipper { get; set; } = null!;
    }
}