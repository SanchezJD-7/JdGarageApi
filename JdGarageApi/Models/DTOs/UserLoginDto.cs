using System.ComponentModel.DataAnnotations;

namespace JdGarageApi.Models.DTOs
{
    public class UserLoginDto
    {
        public string Identifier { get; set; }
        public string Password { get; set; }
    }

}
