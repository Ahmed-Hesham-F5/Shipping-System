using System.ComponentModel.DataAnnotations;

namespace ShippingSystem.DTOs.AuthenticationDTOs
{
    public class RequestForgetPasswordDto
    {
        [Required, EmailAddress]
        public string Email { get; set; } = string.Empty;
        [Required, Url]
        public string ResetPasswordUrl { get; set; } = string.Empty;
    }
}
