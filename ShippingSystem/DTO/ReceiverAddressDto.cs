namespace ShippingSystem.DTO
{
    public class ReceiverAddressDto
    {
        public string Street { get; set; } = null!;
        public string City { get; set; } = null!;
        public string Country { get; set; } = null!;
        public string? Details { get; set; }
    }
}
