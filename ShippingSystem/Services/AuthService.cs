using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using ShippingSystem.DTO;
using ShippingSystem.Interfaces;
using ShippingSystem.Models;
using ShippingSystem.Responses;
using ShippingSystem.Settings;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ShippingSystem.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IShipperRepository _shipperRepository;
        private readonly IOptions<JWT> _jwt;

        public AuthService(UserManager<ApplicationUser> userManager,
            IShipperRepository shipperRepository, IOptions<JWT> jwt)
        {
            _userManager = userManager;
            _shipperRepository = shipperRepository;
            _jwt = jwt;
        }

        public async Task<AuthResponse> RegisterAsync(RegisterDto registerDto)
        {
            if (await _shipperRepository.IsEmailExistAsync(registerDto.Email))
                return new AuthResponse { Message = "Email is already registered!" };

            var isShipperAdded = await _shipperRepository.AddShipperAsync(registerDto);

            if (!isShipperAdded)
                return new AuthResponse { Message = "Failed to register shipper!" };

            var user = await _userManager.FindByEmailAsync(registerDto.Email);

            var userRoles = await _userManager.GetRolesAsync(user!);

            var jwtSecurityToken = await CreateJwtToken(user!);

            return new AuthResponse
            {
                UserName = user!.UserName!,
                Email = user!.Email!,
                IsAuthenticated = true,
                Message = "User registered successfully!",
                Roles = userRoles.ToList(),
                Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken),
                ExpiresOn = jwtSecurityToken.ValidTo
            };
        }

        public async Task<AuthResponse> LoginAsync(LoginDto loginDto)
        {
            var user = await _userManager.FindByEmailAsync(loginDto.Email);

            if (user == null || !await _userManager.CheckPasswordAsync(user, loginDto.Password))
                return new AuthResponse { Message = "Email or password is incorrect!" };

            var jwtSecurityToken = await CreateJwtToken(user);

            var userRoles = await _userManager.GetRolesAsync(user);

            return new AuthResponse
            {
                UserName = user.UserName!,
                Email = user.Email!,
                IsAuthenticated = true,
                Message = "User logged in successfully!",
                Roles = userRoles.ToList(),
                Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken),
                ExpiresOn = jwtSecurityToken.ValidTo
            };
        }

        private async Task<JwtSecurityToken> CreateJwtToken(ApplicationUser user)
        {
            var userClaims = await _userManager.GetClaimsAsync(user);
            var roles = await _userManager.GetRolesAsync(user);
            var roleClaims = new List<Claim>();

            foreach (var role in roles)
                roleClaims.Add(new Claim("roles", role));

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id!),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email!),
                new Claim(JwtRegisteredClaimNames.Name, user.UserName!),
                new Claim(JwtRegisteredClaimNames.Iat, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64)
            }
            .Union(userClaims)
            .Union(roleClaims);

            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwt.Value.Key));
            var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

            var jwtSecurityToken = new JwtSecurityToken(
                issuer: _jwt.Value.Issuer,
                audience: _jwt.Value.Audience,
                claims: claims,
                expires: DateTime.Now.AddMinutes(_jwt.Value.DurationInMinutes),
                signingCredentials: signingCredentials);

            return jwtSecurityToken;
        }
    }
}
