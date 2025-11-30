namespace ShippingSystem.Models
{
    public class PickupCoveredGovernorate
    {
        public int HubId { get; set; }
        public Hub Hub { get; set; } = null!;

        public int GovernorateId { get; set; }
        public Governorate Governorate { get; set; } = null!;

        public decimal PickupCost { get; set; }
    }
}
