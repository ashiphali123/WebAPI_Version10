using System.Text;
using WebAPI_10.DTOs;
using WebAPI_10.Models;
using WebAPI_10.Services;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;

namespace WebAPI_10
{
    public class TokenService
    {
        private readonly IConfiguration _configuration;
        public TokenService(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public string GenerateToken(User user)
        {
            // fro configuration
            var secretKey = _configuration["JwtSetting:SecretKey"];
            var issuer = _configuration["JwtSetting:Issuer"];
            var audience = _configuration["JwtSetting:Audience"];
            var expiryminute = double.Parse(_configuration["JwtSetting:ExpiryMinutes"]!);

            // security key
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey!));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            // claims
            var claim = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email!),
                new Claim(JwtRegisteredClaimNames.UniqueName, user.UserName!),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            };

            // Create the token
            var token = new JwtSecurityToken
            (
                issuer: issuer,
                audience: audience,
                claims: claim,
                signingCredentials: credentials
            );

            // Return the token
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
