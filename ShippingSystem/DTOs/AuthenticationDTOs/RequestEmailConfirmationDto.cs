using System.ComponentModel.DataAnnotations;

namespace ShippingSystem.DTOs.AuthenticationDTOs
{
    public class RequestEmailConfirmationDto
    {
        [Required, EmailAddress]
        public string Email { get; set; } = string.Empty;
        [Required, Url]
        public string ConfirmEmailUrl { get; set; } = string.Empty;
    }
}
