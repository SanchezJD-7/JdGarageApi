using System.ComponentModel.DataAnnotations;

namespace JdGarageApi.Models.DTOs
{
    public class CreateBikeCategoryDto
    {
        [Required(ErrorMessage = "El nombre de la categoría es obligatorio")]
        [MaxLength(30, ErrorMessage = "El nombre no puede superar los 30 caracteres")]
        public string CategoryName { get; set; }
    }
}
