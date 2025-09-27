namespace ShippingSystem.DTOs
{
    public class ShipmentListDto
    {
        public int Id { get; set; }
        public string ReceiverName { get; set; } = null!;
        public string ReceiverPhone { get; set; } = null!;
        public ReceiverAddressDto ReceiverAddress { get; set; } = null!;
        public string ShipmentDescription { get; set; } = null!;
        public bool ExpressDeliveryEnabled { get; set; }
        public decimal CollectionAmount { get; set; }
        public DateTime CreatedAt { get; set; }
        public LatestShipmentStatusDto? LatestShipmentStatus { get; set; }
    }
}
