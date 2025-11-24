using System.ComponentModel.DataAnnotations;

namespace ShippingSystem.DTOs.ShipperDTOs
{
    public class UpdateCompanyInfoDto
    {
        [Required]
        public string CompanyName { get; set; } = null!;
        public string? CompanyLink { get; set; }
        public string? TypeOfProduction { get; set; }
    }
}
