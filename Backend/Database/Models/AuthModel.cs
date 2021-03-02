using System.ComponentModel.DataAnnotations;

namespace Database.Models
{
    public class AuthModel
    {
        [Required]
        public string Email { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
