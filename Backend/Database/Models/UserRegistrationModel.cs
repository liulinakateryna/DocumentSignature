namespace Database.Models
{
    public class UserRegistrationModel : AuthModel
    {
        public string Role { get; set; }

        public string Name { get; set; }
    }
}
