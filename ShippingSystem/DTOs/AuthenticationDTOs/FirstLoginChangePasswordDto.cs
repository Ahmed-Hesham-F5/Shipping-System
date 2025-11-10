using System.ComponentModel.DataAnnotations;

namespace ShippingSystem.DTOs.AuthenticationDTOs
{
    public class FirstLoginChangePasswordDto
    {
        [Required, EmailAddress]
        public string Email { get; set; } = string.Empty;
        [Required, DataType(DataType.Password), MinLength(8), MaxLength(50)]
        public string CurrentPassword { get; set; } = string.Empty;
        [Required, DataType(DataType.Password), MinLength(8), MaxLength(50)]
        public string NewPassword { get; set; } = string.Empty;
        [DataType(DataType.Password)]
        [Compare("NewPassword", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; } = string.Empty;
    }
}
