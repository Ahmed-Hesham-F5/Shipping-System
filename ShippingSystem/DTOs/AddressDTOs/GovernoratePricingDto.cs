using System.ComponentModel.DataAnnotations;

namespace ShippingSystem.DTOs.AddressDTOs
{
    public class GovernoratePricingDto
    {
        [Required]
        public int GovernorateId { get; set; }
        public decimal Cost { get; set; }
    }
}
