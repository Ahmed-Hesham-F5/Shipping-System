using ShippingSystem.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShippingSystem.Models
{
    public class RequestBase
    {
        public int Id { get; set; }
        [ForeignKey("Shipper")]
        public string ShipperId { get; set; } = null!;
        public Shipper Shipper { get; set; } = null!;
        public RequestTypeEnum RequestType { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public int ShipmentsCount { get; set; }
        public RequestStatusEnum RequestStatus { get; set; }
    }
}
