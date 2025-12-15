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
        public string Price { get; set; }
        public string Mileage { get; set; }
        public string Location { get; set; }
        public string FuelType { get; set; }
        public string TransmissionType { get; set; }
        public string OwnerContact { get; set; }
        public string Condition { get; set; }
        public string PlateLastDigit { get; set; }
        public bool Armor { get; set; }
        public int NumberOfOwners { get; set; }
        public bool Sinister { get; set; }
        public string Extras { get; set; }
        public DateTime SoatDate { get; set; }
        public DateTime TechnicalInspection { get; set; }
        public string Engine { get; set; }
        public string Torque { get; set; }
        public string Weight { get; set; }
        public string Power { get; set; }
        public string ImageUrl { get; set; }
        public DateTime CreationDate { get; set; }
        public string ClassificationDisplacement { get; set; }
        public int BikeCategoryId { get; set; }
        [ForeignKey(nameof(BikeCategoryId))]
        public BikeCategory BikeCategory { get; set; }
    }
}
