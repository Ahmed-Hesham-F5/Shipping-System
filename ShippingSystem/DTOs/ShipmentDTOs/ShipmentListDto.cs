namespace ShippingSystem.DTOs.ShipmentDTOs
{
    public class ShipmentListDto
    {
        public int Id { get; set; }
        public string CustomerName { get; set; } = null!;
        public string CustomerPhone { get; set; } = null!;
        public string  City { get; set; } = null!;
        public string Governorate { get; set; } = null!;
        public string ShipmentDescription { get; set; } = null!;
        public bool ExpressDeliveryEnabled { get; set; }
        public decimal CollectionAmount { get; set; }
        public DateTime CreatedAt { get; set; }
        public LatestShipmentStatusDto? LatestShipmentStatus { get; set; }
    }
}
