using System.ComponentModel.DataAnnotations;

namespace ShippingSystem.DTO
{
    public class RegisterDto
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

        [Required, MaxLength(100)]
        public string CompanyName { get; set; } = null!;

        [MaxLength(255)]
        public string? CompanyLink { get; set; }

        [Required, MaxLength(50)]
        public string City { get; set; } = null!;

        [Required, MaxLength(100)]
        public string Street { get; set; } = null!;

        [MaxLength(50)]
        public string Country { get; set; } = "Egypt";

        [MaxLength(500)]
        public string? Details { get; set; }

        [MaxLength(255)]
        public string? TypeOfProduction { get; set; }

        [Required, DataType(DataType.Password), MinLength(8), MaxLength(50)]
        public string Password { get; set; } = null!;

        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; } = null!;
    }
}
