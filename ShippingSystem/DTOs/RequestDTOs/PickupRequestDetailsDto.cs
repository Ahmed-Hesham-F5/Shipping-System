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

        public DateOnly PickupDate { get; set; }
        public TimeOnly WindowStart { get; set; }
        public TimeOnly WindowEnd { get; set; }
        public AddressDto PickupAddress { get; set; } = null!;
        public string ContactName { get; set; } = null!;
        public string ContactPhone { get; set; } = null!;
        public int ShipmentsCount { get; set; }
        public List<ShipmentListDto> Shipments { get; set; } = new List<ShipmentListDto>();
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
