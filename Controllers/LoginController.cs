using LearningAuthenticationAndAuthorization.Models;
using LearningAuthenticationAndAuthorization.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LearningAuthenticationAndAuthorization.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ILogger<LoginController> _logger;
        private readonly ITokenProviderService _tokenProviderService;

        // dictionary to store refresh tokens 
        private static Dictionary<string, User> _refreshTokens = new Dictionary<string, User>();
        public LoginController(IUserService userService,
            ILogger<LoginController> logger, ITokenProviderService tokenProviderService)
        {
            _userService = userService;
            _logger = logger;
            _tokenProviderService = tokenProviderService;
        }

        [HttpPost]
        [AllowAnonymous]
        public IActionResult Login([FromBody] LoginRequestModel loginRequestModel)
        {
            var user = _userService.GetUser(loginRequestModel);

            if (user == null)
            {
                return NotFound("User Not found");
            }

            _logger.LogInformation($"User found");

            var accessToken = _tokenProviderService.GenerateAccessToken(user);

            // adding refresh token/guid
            var refreshToken = Guid.NewGuid().ToString();

            // Store the refresh token (in-memory for simplicity)
            _refreshTokens[refreshToken] = user;

            return Ok(new { AccessToken = accessToken, RefreshToken = refreshToken });
        }

        [HttpPost("refresh")]
        [AllowAnonymous]
        public IActionResult Refresh([FromBody] RefreshRequestModel refreshRequestModel)
        {
            if (_refreshTokens.TryGetValue(refreshRequestModel.RefreshToken, out var user))
            {
                // Generate a new access token
                var accessToken = _tokenProviderService.GenerateAccessToken(user);

                // Remove old refresh token/guid and add new refresh token/guid
                _refreshTokens.Remove(refreshRequestModel.RefreshToken);

                var refreshToken = Guid.NewGuid().ToString();
                // Store the refresh token (in-memory for simplicity)
                _refreshTokens[refreshToken] = user;
                return Ok(new { AccessToken = accessToken, RefreshToken = refreshToken });
            }

            return BadRequest("Invalid refresh token");
        }

    }
}
