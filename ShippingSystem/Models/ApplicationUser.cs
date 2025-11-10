using Microsoft.AspNetCore.Identity;
using ShippingSystem.Enums;

namespace ShippingSystem.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public AccountStatus AccountStatus { get; set; } = AccountStatus.Active;
        public RolesEnum Role { get; set; }
        public bool MustChangePassword { get; set; } = false;
        public int AccessTokenVersion { get; set; } = 0;
        public ICollection<UserPhone>? Phones { get; set; } = new List<UserPhone>();
        public ICollection<UserAddress>? Addresses { get; set; } = new List<UserAddress>();
        public ICollection<RefreshToken>? RefreshTokens { get; set; } = new List<RefreshToken>();
    }
}
