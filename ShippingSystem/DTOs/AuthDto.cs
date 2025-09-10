using System.Text.Json.Serialization;

namespace ShippingSystem.DTOs
{
    public class AuthDTO
    {
        public bool IsAuthenticated { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public List<string> Roles { get; set; } = new List<string>();
        public string Token { get; set; } = string.Empty;
        public DateTime ExpiresOn { get; set; } 
        [JsonIgnore]
        public string? RefreshToken { get; set; }
        public DateTime RefreshTokenExpiration { get; set; }
    }
}
