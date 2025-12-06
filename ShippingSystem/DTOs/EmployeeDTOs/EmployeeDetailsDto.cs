using ShippingSystem.DTOs.AddressDTOs;

namespace ShippingSystem.DTOs.EmployeeDTOs
{
    public class EmployeeDetailsDto
    {
        public string EmployeeId { get; set; } = null!;
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string AccountStatus { get; set; } = null!;
        public string Role { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
        public string PhoneNumber { get; set; } = null!;
        public string Email { get; set; } = null!;
        public AddressDto? Address { get; set; }
        public string? HubName { get; set; }
    }
}
