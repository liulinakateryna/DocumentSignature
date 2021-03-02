using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace Database.Entities
{
    public class User : IdentityUser
    {
        public string Name { get; set; }
        public int Points { get; set; }
        public string Role { get; set; }

        public List<Contract> Contracts { get; set; }

        public User()
        {
            this.Contracts = new List<Contract>();
        }
    }
}
