using System.ComponentModel.DataAnnotations;

namespace ShippingSystem.DTOs.ShipperDTOs
{
    public class ShipperProfileDto
    {
        public string ShipperId { get; set; } = null!;
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public List<string> Phones { get; set; } = new List<string>();
        public List<ShipperAddressListDto> Addresses { get; set; } = new List<ShipperAddressListDto>();

        public string CompanyName { get; set; } = string.Empty;
        public string? CompanyLink { get; set; }
        public string? TypeOfProduction { get; set; }
    }
}
