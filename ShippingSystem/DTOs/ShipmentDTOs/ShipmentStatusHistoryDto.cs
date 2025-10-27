namespace ShippingSystem.DTOs.ShipmentDTOs
{
    public class ShipmentStatusHistoryDto
    {
        public string Status { get; set; } = null!;
        public DateTime Timestamp { get; set; }
        public string? Notes { get; set; }
    }
}
