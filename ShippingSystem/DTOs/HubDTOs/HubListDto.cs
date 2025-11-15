using ShippingSystem.Enums;

namespace ShippingSystem.DTOs.HubDTOs
{
    public class HubListDto
    {
        public int Id { get; set; }
        public string Type { get; set; } = null!;
        public string Name { get; set; } = null!;
        public decimal AreaInSquareMeters { get; set; }
        public HubStatusEnum HubStatus { get; set; }
        public string PhoneNumber { get; set; } = null!;
        public string ManagerName { get; set; } = null!;
        public int EmployeeCount { get; set; }
    }
}
