using LearningAuthenticationAndAuthorization.Models;

namespace LearningAuthenticationAndAuthorization.Services
{
    public interface IUserService
    {
        public User GetUser(LoginRequestModel loginRequestModel);
    }
}
