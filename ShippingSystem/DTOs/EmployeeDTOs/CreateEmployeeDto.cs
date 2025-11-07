using System.ComponentModel.DataAnnotations;

namespace ShippingSystem.DTOs.EmployeeDTOs
{
    public class CreateEmployeeDto
    {
        [Required, MaxLength(50)]
        public string FirstName { get; set; } = null!;

        [Required, MaxLength(50)]
        public string LastName { get; set; } = null!;

        [Required, EmailAddress, MaxLength(255)]
        public string Email { get; set; } = null!;

        [Required]
        [MaxLength(11, ErrorMessage = "Phone number must be 11 digits.")]
        [RegularExpression(@"^(010|011|012|015)\d{8}$", ErrorMessage = "Phone number must start with 010, 011, 012 or 015.")]
        public string PhoneNumber { get; set; } = null!;
        
        [Required, MaxLength(50)]
        public string Role { get; set; } = null!;
        public int? HubId { get; set; } = null;
    }
}
