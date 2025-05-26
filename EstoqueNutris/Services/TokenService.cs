using EstoqueNutris.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace EstoqueNutris.Services
{
    public class TokenService(IConfiguration configuration)
    {
        public string GenerateToken(ApplicationUser user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(JwtRegisteredClaimNames.Email, user.Email ?? ""),
                new Claim("nome", user.Nome),
                new Claim("EscolaID", user.EscolaId ?? string.Empty),
                new Claim(ClaimTypes.Role,user.IsAdmin? "Admin": "User"),
            };

            var jwtKey = configuration["Jwt:Key"];
            if (string.IsNullOrWhiteSpace(jwtKey))
                throw new InvalidOperationException("JWT key not found in configuration.");

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: configuration["Jwt:Issuer"],
                audience: configuration["Jwt:Issuer"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(2),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
