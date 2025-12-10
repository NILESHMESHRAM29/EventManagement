using EventManagement.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace EventManagement.Service
{
    public class JwtService
    {
        private readonly IConfiguration _config;

        public JwtService(IConfiguration config)
        {
            _config = config;
        }

        public string GenerateAccessToken(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),  // Standard claim
                new Claim("userId", user.Id.ToString()),                    // ✅ Custom claim for your controllers
                new Claim(ClaimTypes.Email, user.Email ?? string.Empty)
            };

            if (!string.IsNullOrWhiteSpace(user?.Role?.Name))
            {
                claims.Add(new Claim(ClaimTypes.Role, user.Role.Name));
            }

            var keyString = _config["Jwt:Key"] ?? throw new InvalidOperationException("Missing configuration: Jwt:Key");
            byte[] keyBytes;
            try
            {
                keyBytes = Convert.FromBase64String(keyString);
            }
            catch (FormatException)
            {
                keyBytes = Encoding.UTF8.GetBytes(keyString);
            }

            if (keyBytes.Length < 32)
                throw new InvalidOperationException($"Jwt:Key is too short ({keyBytes.Length} bytes). Provide at least 32 bytes (256 bits).");

            var key = new SymmetricSecurityKey(keyBytes);
            var algorithm = keyBytes.Length >= 64 ? SecurityAlgorithms.HmacSha512 : SecurityAlgorithms.HmacSha256;
            var creds = new SigningCredentials(key, algorithm);

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(15),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public string GenerateRefreshToken()
        {
            var randomBytes = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomBytes);
            }
            return Convert.ToBase64String(randomBytes);
        }
    }
}
