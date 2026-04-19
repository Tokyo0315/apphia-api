using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Apphia_Website_API.Repository.Configuration;
using Apphia_Website_API.Repository.Interface;
using Apphia_Website_API.Repository.ViewModel.UserManagement;
using System.Text.Json;

namespace Apphia_Website_API.Repository.Configuration.Helper {
    public class JwtHelper : IJwtHelper {
        private readonly IConfiguration _config;
        public JwtHelper(IConfiguration config) { _config = config; }

        public string GenerateJwtToken(string userId, RoleReadViewModel role, string email = "", string fullName = "") {
            var jwtSetting = new JwtSettings();
            _config.GetSection("Jwt").Bind(jwtSetting);
            var claims = new List<Claim> {
                new Claim(JwtRegisteredClaimNames.Sub, userId),
                new Claim(ClaimTypes.Role, role.Id.ToString()),
                new Claim(ClaimTypes.Name, role.Name),
                new Claim("UserId", userId),
                new Claim(ClaimTypes.Email, email),
                new Claim("FullName", fullName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSetting.Key));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                issuer: jwtSetting.Issuer, audience: jwtSetting.Audience, claims: claims,
                expires: DateTime.Now.AddMinutes(jwtSetting.ExpirationMinutes), signingCredentials: creds);
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public string TokenDecoder(HttpContext context) {
            var token = context.Request.Cookies["AccessToken"];
            if (token == null) throw new UnauthorizedAccessException();
            var parts = token.Split('.');
            if (parts.Length < 2) throw new ArgumentException("Invalid JWT token format.", nameof(token));
            var payload = Base64UrlDecode(parts[1]);
            return JsonSerializer.Serialize(JsonSerializer.Deserialize<Dictionary<string, object>>(payload));
        }

        private string Base64UrlDecode(string input) {
            string output = input.Replace('-', '+').Replace('_', '/');
            switch (output.Length % 4) {
                case 2: output += "=="; break;
                case 3: output += "="; break;
            }
            return Encoding.UTF8.GetString(Convert.FromBase64String(output));
        }
    }
}
