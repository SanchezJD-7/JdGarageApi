using System.ComponentModel.DataAnnotations;

namespace JdGarageApi.Models.DTOs
{
    public class UserRegisterDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string DocumentType { get; set; }
        public string DocumentNumber { get; set; }
        public bool RequestAdminRole { get; set; }
        public DateTime? BirthDate { get; set; }
        public string State { get; set; }
        public string City { get; set; }
        public string Address { get; set; }
    }
}
