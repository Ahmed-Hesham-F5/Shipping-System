using ShippingSystem.Enums;

namespace ShippingSystem.Models
{
    public class RescheduleRequest : RequestBase
    {
        public int ScheduledRequestId { get; set; }
        public RequestTypeEnum ScheduledRequestType { get; set; }
        public DateOnly OldRequestDate { get; set; }
        public TimeOnly OldTimeWindowStart { get; set; }
        public TimeOnly OldTimeWindowEnd { get; set; }
        public DateOnly NewRequestDate { get; set; }
        public TimeOnly NewTimeWindowStart { get; set; }
        public TimeOnly NewTimeWindowEnd { get; set; }
        public string? Reason { get; set; }
    }
}
