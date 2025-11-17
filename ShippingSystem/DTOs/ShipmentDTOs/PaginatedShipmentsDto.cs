namespace ShippingSystem.DTOs.ShipmentDTOs
{
    public class PaginatedShipmentsDto
    {
        public int TotalCount { get; set; }
        public List<ShipmentListDto> Shipments { get; set; } = new();
    }
}
