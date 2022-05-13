using JWT;
using JWT.Algorithms;
using JWT.Builder;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using UserApi.DTO;
using UserApi.Models;

namespace UserApi.Filters
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IConfiguration _config;
        public AuthenticationService (IConfiguration config)
        {
            _config = config;
        }

        public string GenerateToken(UserDTO user)
        {
            //Add DB checking here
            var secret = _config.GetSection("JwtKey:secret").Value;
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));

            var issuer = "https://localhost";
            var audience = "http://127.0.0.1:5500";

            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new System.Security.Claims.ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.NameIdentifier, user.name)
                }),
                Expires = DateTime.UtcNow.AddMinutes(30),
                Issuer = issuer,
                Audience = audience,
                SigningCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
    }
}
