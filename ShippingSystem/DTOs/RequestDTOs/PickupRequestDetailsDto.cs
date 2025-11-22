using ShippingSystem.DTOs.AddressDTOs;
using ShippingSystem.DTOs.ShipmentDTOs;

namespace ShippingSystem.DTOs.RequestDTOs
{
    public class PickupRequestDetailsDto
    {
        public int Id { get; set; }
        public string RequestType { get; set; } = null!;
        public string RequestStatus { get; set; } = null!;
        public string CreatedBy { get; set; } = null!;
        public string Role { get; set; } = null!;
        public AddressDto PickupAddress { get; set; } = null!;
        public List<ShipmentListDto> Shipments { get; set; } = new List<ShipmentListDto>();
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
