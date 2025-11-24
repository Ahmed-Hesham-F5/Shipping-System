using System.ComponentModel.DataAnnotations;

namespace ShippingSystem.DTOs.EmailDTOs
{
    public class ChangeEmailRequestDto
    {
        [Required, MaxLength(255), EmailAddress]
        public string NewEmail { get; set; } = null!;
        [Required, Url]
        public string ConfirmNewEmailUrl { get; set; } = null!;
    }
}
