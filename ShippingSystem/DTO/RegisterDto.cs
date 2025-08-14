using System.ComponentModel.DataAnnotations;

namespace ShippingSystem.DTO
{
    public class RegisterDto
    {
        [Required]
        [MaxLength(50)]
        public string FirstName { get; set; } = null!;

        [Required]
        [MaxLength(50)]
        public string LastName { get; set; } = null!;

        [Required]
        [EmailAddress]
        [MaxLength(256)]
        public string Email { get; set; } = null!;

        [Required]
        [MaxLength(15)]
        public string PhoneNumber { get; set; } = null!;

        [Required]
        [MaxLength(100)]
        public string CompanyName { get; set; } = null!;

        [MaxLength(255)]
        public string? CompanyLink { get; set; }

        [Required]
        [MaxLength(50)]
        public string City { get; set; } = null!;

        [Required]
        [MaxLength(100)]
        public string Street { get; set; } = null!;

        [MaxLength(50)]
        public string Country { get; set; } = "Egypt";

        [MaxLength(255)]
        public string? Details { get; set; }

        [MaxLength(255)]
        public string? TypeOfTheProduction { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [MinLength(8)]
        [MaxLength(100)]
        public string Password { get; set; } = null!;

        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; } = null!;
    }
}
