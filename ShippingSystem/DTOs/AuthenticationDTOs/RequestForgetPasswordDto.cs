using System.ComponentModel.DataAnnotations;

namespace ShippingSystem.DTOs.AuthenticationDTOs
{
    public class RequestForgetPasswordDto
    {
        [Required, EmailAddress]
        public string Email { get; set; } = string.Empty;
        public string? FrontendUrl { get; set; } = string.Empty;
    }
}
