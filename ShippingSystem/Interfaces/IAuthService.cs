using ShippingSystem.DTO;
using ShippingSystem.Models;
using ShippingSystem.Responses;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace ShippingSystem.Interfaces
{
    public interface IAuthService
    {
        public void CreateJwtToken(ApplicationUser user, IList<Claim> Userclaims, out string Token, out DateTime ExpiresOn);
        public RefreshToken GenerateRefreshToken();
    }
}
