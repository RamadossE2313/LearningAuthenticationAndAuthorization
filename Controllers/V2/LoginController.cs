using Asp.Versioning;
using LearningAuthenticationAndAuthorization.Models;
using LearningAuthenticationAndAuthorization.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using LearningAuthenticationAndAuthorization.Common;
using Microsoft.AspNetCore.Authentication;

// test
namespace LearningAuthenticationAndAuthorization.Controllers.V2
{
    [Route("api/[controller]")]
    [ApiController]
    [ApiVersion("2.0")]
    public class LoginController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ILogger<LoginController> _logger;
        
        public LoginController(IUserService userService,
            ILogger<LoginController> logger)
        {
            _userService = userService;
            _logger = logger;
        }

        [HttpPost("LoginUsingCookie")]
        [AllowAnonymous]
        public async Task<IActionResult> LoginUsingCookie([FromBody] LoginRequestModel loginRequestModel)
        {
            var user = _userService.GetUser(loginRequestModel);

            if (user == null)
            {
                return NotFound("User Not found");
            }

            _logger.LogInformation($"User found");

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

            var claimsIdentity = new ClaimsIdentity(claims, Constants.COOKIESCHEMENAME);

            var authProperties = new AuthenticationProperties
            {
                IsPersistent = true, // Persist cookie across browser sessions
                AllowRefresh = true,
                ExpiresUtc = DateTime.UtcNow.AddMinutes(30),
            };

            await HttpContext.SignInAsync(
                Constants.COOKIESCHEMENAME,
                new ClaimsPrincipal(claimsIdentity),
                authProperties);

            return Ok("Authenticated SuccessFully");
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="loginRequestModel"></param>
        /// <returns></returns>
        [HttpPost("LoginUsingJWt1")]
        [AllowAnonymous]
        public IActionResult LoginUsingJWt1([FromBody] LoginRequestModel loginRequestModel)
        {
            var user = _userService.GetUser(loginRequestModel);

            if (user == null)
            {
                return NotFound("User Not found");
            }

            _logger.LogInformation($"User found");

            byte[] keyInBytes = Encoding.UTF8.GetBytes(Constants.JWT1SECRETKEY);
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
                issuer: Constants.JWT1ISSUER,
                audience: Constants.JWT1AUDIENCE,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(30),
                signingCredentials: credentials
            );

            var accessToken = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);

            return Ok(new { AccessToken = accessToken});
        }


        [HttpPost("LoginUsingJWt2")]
        [AllowAnonymous]
        public IActionResult LoginUsingJWt2([FromBody] LoginRequestModel loginRequestModel)
        {
            var user = _userService.GetUser(loginRequestModel);

            if (user == null)
            {
                return NotFound("User Not found");
            }

            _logger.LogInformation($"User found");


            byte[] keyInBytes = Encoding.UTF8.GetBytes(Constants.JWT2SECRETKEY);
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
                issuer: Constants.JWT2ISSUER,
                audience: Constants.JWT2AUDIENCE,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(30),
                signingCredentials: credentials
            );

           var accessToken = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);

            return Ok(new { AccessToken = accessToken });
        }
    }
}
