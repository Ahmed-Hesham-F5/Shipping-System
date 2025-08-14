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
        public string? TypeOfTheProduction { get; set; }


     

   

        public ICollection<Phone>? Phones { get; set; } = new List<Phone>();

        public ICollection<Address>? Addresses { get; set; } = new List<Address>();
    }
}
