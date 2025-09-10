using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShippingSystem.Models
{
    public class ShipperAddress
    {
        [Key]
        public int Id { get; set; }
        public string Street { get; set; } = null!;
        public string City { get; set; } = null!;
        public string Country { get; set; } = "Egypt";
        public string? Details { get; set; }
        public string ShipperId { get; set; } = null!;

        [ForeignKey("ShipperId")]
        public Shipper Shipper { get; set; } = null!;
    }
}