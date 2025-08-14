using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShippingSystem.Models
{
    public class Phone
    {


        [Required]
        [MaxLength(15)]
        public string PhoneNumber { get; set; } = null!;

        [Required]
        public string ShipperId { get; set; }

        [ForeignKey("ShipperId")]
        public Shipper Shipper { get; set; } = null!;
    }
}