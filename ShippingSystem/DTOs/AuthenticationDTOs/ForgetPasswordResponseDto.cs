namespace ShippingSystem.DTOs.AuthenticationDTOs
{
    public class ForgetPasswordResponseDto
    {
        public string ResetToken { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
    }
}
