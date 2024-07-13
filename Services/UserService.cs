using LearningAuthenticationAndAuthorization.Models;

namespace LearningAuthenticationAndAuthorization.Services
{
    public class UserService : IUserService
    {
        private readonly List<User> _users;
        public UserService()
        {
            _users = new List<User>()
            {
                new User()
                {
                    Id = 1,
                    Name = "Test",
                    Email = "Test@gmail.com",
                    Gender = "Male",
                    UserName = "Test",
                    Passsword = "Test",
                    Roles = new List<Roles>
                    {
                        new Roles
                        {
                            Id = 1,
                            Name = "User",
                        },
                        new Roles 
                        {
                            Id = 2,
                            Name = "Admin",
                        }
                    }
                },
                new User()
                {
                    Id = 2,
                    Name = "Test1",
                    Email = "Test1@gmail.com",
                    Gender = "Male",
                    UserName = "Test1",
                    Passsword = "Test1",
                    Roles = new List<Roles>
                    {
                        new Roles
                        {
                            Id = 1,
                            Name = "User",
                        }
                    }
                }
            };
        }

        public User GetUser(LoginRequestModel loginRequestModel)
        {
            if (loginRequestModel.UserName == null) throw new ArgumentNullException(nameof(loginRequestModel.UserName));

            if(loginRequestModel.Password == null) throw new ArgumentNullException(nameof(loginRequestModel.Password));

            var user = _users.FirstOrDefault(x => x.UserName == loginRequestModel.UserName && x.Passsword == loginRequestModel.Password);

            if (user == null) throw new UnauthorizedAccessException("Invalid username or password.");

            return user;
        }
    }
}
