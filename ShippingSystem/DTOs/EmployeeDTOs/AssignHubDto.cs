using System.ComponentModel.DataAnnotations;

namespace ShippingSystem.DTOs.EmployeeDTOs
{
    public class AssignHubDto
    {
        [Required]
        public int HubId { get; set; }
    }
}
