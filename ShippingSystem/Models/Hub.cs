using ShippingSystem.Enums;

namespace ShippingSystem.Models
{
    public class Hub
    {
        public int Id { get; set; }
        public HubTypesEnum HubType { get; set; }
        public string Name { get; set; } = null!;
        public Address Address { get; set; } = null!;
        public string HubPhoneNumber { get; set; } = null!;
        public decimal AreaInSquareMeters { get; set; }

        public string? ManagerId { get; set; }
        public Employee? Manager { get; set; }

        public ICollection<Employee>? Employees { get; set; } = new List<Employee>();
        public ICollection<Shipment>? Shipments { get; set; } = new List<Shipment>();
    }
}
