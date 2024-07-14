using Asp.Versioning;
using LearningAuthenticationAndAuthorization.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LearningAuthenticationAndAuthorization.Controllers.V2
{
    [ApiController]
    [Route("api/[controller]")]
    [ApiVersion("2.0")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// This method will called by any scheme, cookie, jwt1 and jwt2
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        [Route("GetByAllScheme")]
        public IEnumerable<WeatherForecast> GetByAllScheme()
        {
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }

        /// <summary>
        /// This method will called by any scheme, cookie, jwt1 and jwt2
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        [Route("GetClaimsForLoggedInUser")]
        public Dictionary<string, List<string>> GetClaimsForLoggedInUser()
        {
            // once token is authenticated you can take the user information from the httpcontext.user property

            var user = HttpContext.User;

            Dictionary<string, List<string>> keyValuePairs = new();
            foreach (var item in user.Claims)
            {
                if (!keyValuePairs.TryGetValue(item.Type, out List<string> values))
                {
                    keyValuePairs.Add(item.Type, new List<string>() { item.Value });
                }
                else
                {
                    values.Add(item.Value);
                    keyValuePairs[item.Type] = values;
                }
            }

            return keyValuePairs;
        }

        /// <summary>
        /// This method will called by any scheme, cookie, jwt1 and jwt2
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize(Policy = Constants.COOKIEPOLICYNAME)]
        [Route("GetByCookieScheme")]
        public IEnumerable<WeatherForecast> GetByCookieScheme()
        {
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }


        /// <summary>
        /// This method will called by only JWT scheme jwt1 and jwt2
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize(Policy = Constants.JWTPOLICYENAME)]
        [Route("GetByJWTScheme")]
        public IEnumerable<WeatherForecast> GetByJWTScheme()
        {
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }

        /// <summary>
        /// This method will called by only JWT scheme jwt1 and jwt2
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize(Policy = Constants.JWTADMINPOLICYNAME)]
        [Route("GetByJWTSchemeWithAdminRole")]
        public IEnumerable<WeatherForecast> GetByJWTSchemeWithAdminRole()
        {
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }

        /// <summary>
        /// This method will called by only JWT1 scheme
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize(Policy = Constants.JWT1POLICYENAME)]
        [Route("GetByJWT1Scheme")]
        public IEnumerable<WeatherForecast> GetByJWT1Scheme()
        {
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }

        /// <summary>
        /// This method will called by only JWT1 scheme
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize(Policy = Constants.JWT2POLICYENAME)]
        [Route("GetByJWT2Scheme")]
        public IEnumerable<WeatherForecast> GetByJWT2Scheme()
        {
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }
    }
}
