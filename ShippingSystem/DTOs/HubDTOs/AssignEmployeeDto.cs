using System.ComponentModel.DataAnnotations;

namespace ShippingSystem.DTOs.HubDTOs
{
    public class AssignEmployeeDto
    {
        [Required]
        public string EmployeeId { get; set; } = null!;
    }
}
