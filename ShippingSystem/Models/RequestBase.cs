using ShippingSystem.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShippingSystem.Models
{
    public class RequestBase
    {
        public int Id { get; set; }
        [ForeignKey("User")]
        public string UserId { get; set; } = null!;
        public ApplicationUser User { get; set; } = null!;
        public RequestTypeEnum RequestType { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public RequestStatusEnum RequestStatus { get; set; }
    }
}
