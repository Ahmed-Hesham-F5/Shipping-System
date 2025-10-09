using ShippingSystem.Enums;

namespace ShippingSystem.DTOs
{
    public class RequestListDto
    {
        public int Id { get; set; }
        public string RequestType { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public int ShipmentsCount { get; set; }
        public string Status { get; set; } = null!;
    }
}
