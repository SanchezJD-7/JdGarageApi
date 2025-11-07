using System.ComponentModel.DataAnnotations;

namespace JdGarageApi.Models
{
    public class BikeCategory
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string CategoryName { get; set; }
        [Required]
        public DateTime CreatedDate { get; set; }
    }
}
