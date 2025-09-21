using Application.DTOs.Response;
using Application.Interfaces.IServices;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class JWTService : IJWTService
    {
        private readonly IConfiguration _config;

        public JWTService(IConfiguration config)
        {
            _config = config;
        }
        public string GenerateJwtToken(UserDTORespone user)
        {
            var key = Encoding.UTF8.GetBytes(_config["Jwt:SecretKey"]!);

            var claims = new List<Claim>
        {
            new Claim("UserId", user.UserId.ToString() ?? ""),
            new Claim("Email", user.Email ?? ""),
            new Claim("RoleId", user.RoleId.ToString() ?? "5"),
            new Claim("IsPremium", user.IsPremium.ToString() ?? "false")
        };

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(30),
                Issuer = _config["Jwt:Issuer"],
                Audience = _config["Jwt:Audience"],
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature)
            };


            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
