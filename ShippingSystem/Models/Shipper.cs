using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShippingSystem.Models
{
    public class Shipper
    { 
        [Key, ForeignKey("ApplicationUser")]
        public string ShipperId { get; set; } = null!;
        public ApplicationUser ApplicationUser { get; set; } = null!;

        [Required]
        [MaxLength(100)]
        public string CompanyName { get; set; } = null!;

        [MaxLength(255)]
        public string? CompanyLink { get; set; }

        [MaxLength(255)]
        public string? TypeOfProduction { get; set; }
   
        public ICollection<ShipperPhone>? Phones { get; set; } = new List<ShipperPhone>();
        public ICollection<ShipperAddress>? Addresses { get; set; } = new List<ShipperAddress>();
        public ICollection<Shipment>? Shipments { get; set; } = new List<Shipment>();
    }
}
