using System.ComponentModel.DataAnnotations;

namespace JdGarageApi.Models.DTOs
{
    public class BikeCategoryDto
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "El nombre de la categoría es obligatorio")]
        [MaxLength(30, ErrorMessage = "El nombre no puede superar los 30 caracteres")]
        public string CategoryName { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
