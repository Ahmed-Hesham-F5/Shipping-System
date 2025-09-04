using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using ShippingSystem.Interfaces;
using ShippingSystem.Models;
using ShippingSystem.Settings;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace ShippingSystem.Services
{
    public class AuthService : IAuthService
    {
        private readonly IOptions<JWT> _jwt;

        public AuthService(IOptions<JWT> jwt)
        {
            _jwt = jwt;
        }

        public void CreateJwtToken(ApplicationUser user, IList<Claim> Userclaims,
                                            out string Token, out DateTime ExpiresOn)
        {
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id!),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email!),
                new Claim(JwtRegisteredClaimNames.Name, user.UserName!),
                new Claim(JwtRegisteredClaimNames.Iat,
                    DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64)
            }
            .Union(Userclaims);

            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwt.Value.Key));
            var signingCredentials = new SigningCredentials(symmetricSecurityKey,
                                            SecurityAlgorithms.HmacSha256);

            var jwtSecurityToken = new JwtSecurityToken(
                issuer: _jwt.Value.Issuer,
                audience: _jwt.Value.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(_jwt.Value.DurationInMinutesForAccessToken),
                signingCredentials: signingCredentials);

            ExpiresOn = jwtSecurityToken.ValidTo;
            Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
        }

        public RefreshToken GenerateRefreshToken()
        {
            string _token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(32));

            return new RefreshToken
            {
                Token = _token,
                ExpiresOn = DateTime.UtcNow.AddMinutes(_jwt.Value.DurationInMinutesForRefreshToken),
                CreatedOn = DateTime.UtcNow
            };
        }
    }
}
