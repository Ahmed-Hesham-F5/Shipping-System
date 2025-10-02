using ShippingSystem.Enums;

namespace ShippingSystem.Models
{
    public class ShipperRequestBase
    {
        public int Id { get; set; }
        public int ShipperId { get; set; }
        public ShipperRequestTypeEnum requestType { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
