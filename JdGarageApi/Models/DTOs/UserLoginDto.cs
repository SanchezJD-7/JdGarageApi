using System.ComponentModel.DataAnnotations;

namespace JdGarageApi.Models.DTOs
{
    public class UserLoginDto
    {
        [Required(ErrorMessage = "El usuario es obligatorio")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "La contraseña es obligatoria")]
        public string Password { get; set; }
    }
}
