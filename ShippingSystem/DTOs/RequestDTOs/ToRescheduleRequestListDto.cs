namespace ShippingSystem.DTOs.RequestDTOs
{
    public class ToRescheduleRequestListDto
    {
        public int Id { get; set; }
        public string RequestType { get; set; } = null!;
        public DateOnly RequestDate { get; set; }
        public TimeOnly WindowStart { get; set; }
        public TimeOnly WindowEnd { get; set; }
        public int ShipmentsCount { get; set; }
        public string RequestStatus { get; set; } = null!;
    }
}
