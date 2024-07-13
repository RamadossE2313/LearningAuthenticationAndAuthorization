namespace LearningAuthenticationAndAuthorization.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Gender { get; set; }
        public List<Roles> Roles{ get; set; }
        public string UserName { get; set; }
        public string Passsword { get; set; }
    }
}
