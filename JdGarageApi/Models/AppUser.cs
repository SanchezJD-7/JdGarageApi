using Microsoft.AspNetCore.Identity;

namespace JdGarageApi.Models
{
    public class AppUser : IdentityUser
    {
        public string Name { get; set; }

    }
}
