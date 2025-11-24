using System.ComponentModel.DataAnnotations;

namespace ShippingSystem.DTOs.EmailDTOs
{
    public class ChangeEmailDto
    {
        [Required, MaxLength(255), EmailAddress]
        public string OldEmail { get; set; } = null!;
        [Required, MaxLength(255), EmailAddress]
        public string NewEmail { get; set; } = null!;
        [Required]
        public string Token { get; set; } = null!;
    }
}
