using ShippingSystem.DTOs.AddressDTOs;
using ShippingSystem.DTOs.ShipmentDTOs;
using ShippingSystem.Models;

namespace ShippingSystem.DTOs.RequestDTOs
{
    public class ReturnRequestDetailsDto
    {
        public int Id { get; set; }
        public string RequestType { get; set; } = null!;
        public string RequestStatus { get; set; } = null!;
        public string CreatedBy { get; set; } = null!;
        public string Role { get; set; } = null!;

        public AddressDto CustomerAddress { get; set; } = null!;
        public string CustomertName { get; set; } = null!;
        public string CustomerPhone { get; set; } = null!;
        public string? CustomerEmail { get; set; } = null!;
        public List<ShipmentListDto> Shipments { get; set; } = new List<ShipmentListDto>();
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
