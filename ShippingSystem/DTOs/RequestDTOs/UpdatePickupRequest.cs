using ShippingSystem.DTOs.AddressDTOs;
using System.ComponentModel.DataAnnotations;

namespace ShippingSystem.DTOs.RequestDTOs
{
    public class UpdatePickupRequest
    {
        [Required]
        public AddressDto PickupAddress { get; set; } = null!;
        public List<int> ShipmentIds { get; set; } = new List<int>();
    }
}
