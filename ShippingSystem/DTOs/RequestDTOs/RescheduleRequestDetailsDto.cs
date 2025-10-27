using ShippingSystem.Enums;

namespace ShippingSystem.DTOs.RequestDTOs
{
    public class RescheduleRequestDetailsDto
    {
        public int Id { get; set; }
        public string RequestType { get; set; } = null!;
        public string RequestStatus { get; set; } = null!;
        public string CreatedBy { get; set; } = null!;
        public string Role { get; set; } = null!;

        public int ScheduledRequestId { get; set; }
        public string ScheduledRequestType { get; set; } = null!;
        public DateOnly OldRequestDate { get; set; }
        public TimeOnly OldTimeWindowStart { get; set; }
        public TimeOnly OldTimeWindowEnd { get; set; }
        public DateOnly NewRequestDate { get; set; }
        public TimeOnly NewTimeWindowStart { get; set; }
        public TimeOnly NewTimeWindowEnd { get; set; }
        public int ShipmentsCount { get; set; }
        public string? Reason { get; set; }
    }
}
