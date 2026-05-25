using Microsoft.AspNetCore.Identity;

namespace JdGarageApi.Models
{
    public class AppUser : IdentityUser
    {
        public string Name { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string DocumentType { get; set; }
        public string DocumentNumber { get; set; }
        public DateTime? BirthDate { get; set; }
        public string State { get; set; }
        public string City { get; set; }
        public string Address { get; set; }
    }
}
