namespace ShippingSystem.DTOs
{
    public class ToReturnShipmentListDto
    {
        public int Id { get; set; }
        public string CustomerName { get; set; } = null!;
        public string CustomerPhone { get; set; } = null!;
        public AddressDto CustomerAddress { get; set; } = null!;
        public string ShipmentDescription { get; set; } = null!;
        public decimal ShipmentWeight { get; set; }
        public int Quantity { get; set; }
        public bool ExpressDeliveryEnabled { get; set; }
        public decimal CollectionAmount { get; set; }
        public DateTime CreatedAt { get; set; }
        public LatestShipmentStatusDto? LatestShipmentStatus { get; set; }
    }
}
