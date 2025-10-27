using ShippingSystem.Enums;
using System.ComponentModel.DataAnnotations;

namespace ShippingSystem.DTOs.RequestDTOs
{
    public class CreateRescheduleRequestDto
    {
        [Required]
        public int ScheduledRequestId { get; set; }
        [Required]
        public DateOnly NewRequestDate { get; set; }
        [Required]
        public TimeOnly NewTimeWindowStart { get; set; }
        [Required]
        public TimeOnly NewTimeWindowEnd { get; set; }
        [MaxLength(1000)]
        public string? Reason { get; set; }
    }
}
