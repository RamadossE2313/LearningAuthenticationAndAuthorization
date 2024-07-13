using LearningAuthenticationAndAuthorization.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace LearningAuthenticationAndAuthorization.Services
{
    public class TokenProviderService : ITokenProviderService
    {
        private readonly IConfiguration _configuration;

        public TokenProviderService(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        /// <summary>
        /// This method used to generate token
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public string GenerateAccessToken(User user)
        {
            // Sample value
              //"Jwt": {
                   //"Key": "2dGhY6z4QaL8bTx9N3pRfWvAqZ1cXsVe5jUm7yPdLoI9Kn0hBsC4MxEwJiYtFu",
                   //"Issuer": "LearningAuthenticationAndAuthorizationApi"
              //}

            // Key value is stored in user secret
            var jwtKey = _configuration.GetSection("Jwt:Key").Get<string>();
            if (jwtKey == null)
            {
                throw new ArgumentException(nameof(jwtKey));
            }
            // Issuer value is stored in user secret
            var jwtIssuer = _configuration.GetSection("Jwt:Issuer").Get<string>();
            if (jwtIssuer == null)
            {
                throw new ArgumentException(nameof(jwtKey));
            }

            byte[] keyInBytes = Encoding.UTF8.GetBytes(jwtKey);
            var securityKey = new SymmetricSecurityKey(keyInBytes);

            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>()
            {
                new Claim("name", user.Name),
                new Claim("email", user.Email),
                new Claim("gender", user.Gender),
                new Claim("username", user.UserName),
            };

            if (user.Roles.Count > 0)
            {
                foreach (var role in user.Roles)
                {
                    claims.Add(new Claim("roles", role.Name));
                }
            }

            JwtSecurityToken jwtSecurityToken = new JwtSecurityToken(
                issuer: jwtIssuer,
                audience: jwtIssuer,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(30),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
        }
    }
}
