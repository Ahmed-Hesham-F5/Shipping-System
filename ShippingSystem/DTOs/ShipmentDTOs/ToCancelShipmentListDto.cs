namespace ShippingSystem.DTOs.ShipmentDTOs
{
    public class ToCancelShipmentListDto
    {
        public int ShipmentId { get; set; }
        public string CustomerName { get; set; } = null!;
        public string ShipmentDescription { get; set; } = null!;
        public int Quantity { get; set; }
        public decimal ShipmentWeight { get; set; }
        public decimal CollectionAmount { get; set; }
        public int RequestId { get; set; }
        public string RequestType { get; set; } = null!;
    }
}
