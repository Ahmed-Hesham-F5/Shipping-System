using ShippingSystem.DTOs.AddressDTOs;
using ShippingSystem.DTOs.EmployeeDTOs;

namespace ShippingSystem.DTOs.HubDTOs
{
    public class HubProfileDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Type { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
        public string HubStatus { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;
        public decimal AreaInSquareMeters { get; set; }

        public AddressDto Address { get; set; } = null!;

        public ICollection<EmployeeDetailsDto>? Employees { get; set; } = new List<EmployeeDetailsDto>();
        public ICollection<CoveredGovernorateDetailsDto>? PickupCoveredGovernorates { get; set; } = new List<CoveredGovernorateDetailsDto>();
        public ICollection<CoveredGovernorateDetailsDto>? DeliveryCoveredGovernorates { get; set; } = new List<CoveredGovernorateDetailsDto>();
    }
}
