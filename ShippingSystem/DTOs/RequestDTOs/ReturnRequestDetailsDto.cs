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

        public DateOnly ReturnPickupDate { get; set; }
        public TimeOnly ReturnPickupWindowStart { get; set; }
        public TimeOnly ReturnPickupWindowEnd { get; set; }
        public Address ReturnPickupAddress { get; set; } = null!;
        public string CustomerContactName { get; set; } = null!;
        public string CustomerContactPhone { get; set; } = null!;
        public int ShipmentsCount { get; set; }
        public List<ShipmentListDto> Shipments { get; set; } = new List<ShipmentListDto>();
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
