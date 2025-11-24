using System.ComponentModel.DataAnnotations;

namespace ShippingSystem.DTOs.ShipperDTOs
{
    public class UpdateShipperNameDto
    {
        [Required, MaxLength(50), MinLength(2)]
        public string FirstName { get; set; } = null!;

        [Required, MaxLength(50), MinLength(2)]
        public string LastName { get; set; } = null!;
    }
}
