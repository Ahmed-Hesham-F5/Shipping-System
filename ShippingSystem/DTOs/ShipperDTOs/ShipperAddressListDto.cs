namespace ShippingSystem.DTOs.ShipperDTOs
{
    public class ShipperAddressListDto
    {
        public int Id { get; set; }
        public string City { get; set; } = null!;
        public string Street { get; set; } = null!;
        public string Governorate { get; set; } = null!;
        public string? Details { get; set; } = null!;
        public string? GoogleMapAddressLink { get; set; } = null!;
    }
}
