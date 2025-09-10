using ShippingSystem.Models;
using System.Security.Claims;

namespace ShippingSystem.Interfaces
{
    public interface IAuthService
    {
        public void CreateJwtToken(ApplicationUser user, IList<Claim> Userclaims, out string Token, out DateTime ExpiresOn);
        public RefreshToken GenerateRefreshToken();
    }
}
