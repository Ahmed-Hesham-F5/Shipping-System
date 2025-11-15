using ShippingSystem.DTOs.AddressDTOs;
using ShippingSystem.DTOs.ShipmentDTOs;

namespace ShippingSystem.DTOs.RequestDTOs
{
    public class ExchangeRequestDetailsDto
    {
        public int Id { get; set; }
        public string RequestType { get; set; } = null!;
        public string RequestStatus { get; set; } = null!;
        public string CreatedBy { get; set; } = null!;
        public string Role { get; set; } = null!;

        public AddressDto PickupAddress { get; set; } = null!;
        public AddressDto CustomerAddress { get; set; } = null!;
        public string CustomerName { get; set; } = null!;
        public string CustomerPhone { get; set; } = null!;
        public string? CustomerEmail { get; set; } = null!;
        public string? ExchangeReason { get; set; }

        public int ShipmentsCount { get; set; }
        public List<ShipmentListDto> FromCustomer { get; set; } = new List<ShipmentListDto>();
        public List<ShipmentListDto> ToCustomer { get; set; } = new List<ShipmentListDto>();

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
