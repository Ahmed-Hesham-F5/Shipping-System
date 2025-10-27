namespace ShippingSystem.DTOs.RequestDTOs
{
    public class CreateCancellationRequestDto
    {
        public List<int> ShipmentIds { get; set; } = new List<int>();
    }
}
