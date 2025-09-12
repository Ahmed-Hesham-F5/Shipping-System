namespace ShippingSystem.DTOs
{
    public class ShipmentStatusDto
    {
        public int Id { get; set; }
        public string Status { get; set; } = null!;
        public DateTime Timestamp { get; set; }
        public string? Notes { get; set; }
    }
}
