using Microsoft.AspNetCore.Identity;
using ShippingSystem.Enums;

namespace ShippingSystem.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public AccountStatus AccountStatus { get; set; } = AccountStatus.Active;
        public ICollection<RefreshToken>? RefreshTokens { get; set; } = new List<RefreshToken>();
    }
}
