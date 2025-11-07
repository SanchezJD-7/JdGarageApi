using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JdGarageApi.Models
{
    public class Bike
    {
        [Key]
        public int Id { get; set; }
        public string Brand { get; set; }
        public string Line { get; set; }
        public string Displacement { get; set; }
        public string Model { get; set; }
        public string Color { get; set; }
        public string? UrlImage { get; set; }
        public string? UrlLocalImage { get; set; }
        public DateTime CreationDate { get; set; }
        public string ClassificationDisplacement { get; set; }

        //Relación con BikeCategory
        public int BikeCategoryId { get; set; }
        [ForeignKey("BikeCategoryId")]
        public BikeCategory BikeCategory { get; set; }
    }
}
