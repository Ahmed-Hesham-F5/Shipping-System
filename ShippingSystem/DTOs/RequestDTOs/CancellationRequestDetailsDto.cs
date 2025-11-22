using ShippingSystem.DTOs.ShipmentDTOs;

namespace ShippingSystem.DTOs.RequestDTOs
{
    public class CancellationRequestDetailsDto
    {
        public int Id { get; set; }
        public string RequestType { get; set; } = null!;
        public string Status { get; set; } = null!;
        public string CreatedBy { get; set; } = null!;
        public string Role { get; set; } = null!;
        public List<ShipmentListDto> Shipments { get; set; } = new List<ShipmentListDto>();
    }
}
