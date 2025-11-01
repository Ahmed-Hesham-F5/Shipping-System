using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShippingSystem.Models
{
    public class Shipper
    { 
        [Key, ForeignKey("User")]
        public string ShipperId { get; set; } = null!;
        public ApplicationUser User { get; set; } = null!;

        public string CompanyName { get; set; } = null!;
        public string? CompanyLink { get; set; }
        public string? TypeOfProduction { get; set; }
   
        public ICollection<ShipperAddress>? Addresses { get; set; } = new List<ShipperAddress>();
        public ICollection<Shipment>? Shipments { get; set; } = new List<Shipment>();
    }
}
