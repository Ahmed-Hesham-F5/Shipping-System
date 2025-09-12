using ShippingSystem.Enums;

namespace ShippingSystem.Models
{
    public class ShippingSetting
    {
        public int Id { get; set; }
        public ShippingSettingKeys Key { get; set; }
        public string Value { get; set; } = null!;
    }
}
