using LearningAuthenticationAndAuthorization.Models;

namespace LearningAuthenticationAndAuthorization.Services
{
    public interface ITokenProviderService
    {
        /// <summary>
        /// This method used to generate access token using user information
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        string GenerateAccessToken(User user);
    }
}
