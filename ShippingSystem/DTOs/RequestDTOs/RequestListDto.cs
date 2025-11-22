namespace ShippingSystem.DTOs.RequestDTOs
{
    public class RequestListDto
    {
        public int Id { get; set; }
        public string RequestType { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string RequestStatus { get; set; } = null!;
    }
}
