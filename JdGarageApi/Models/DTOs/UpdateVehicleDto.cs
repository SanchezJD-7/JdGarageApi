namespace JdGarageApi.Models.DTOs
{
    public class UpdateVehicleDto
    {
        public int Id { get; set; }

        public string? Brand { get; set; }
        public string? Line { get; set; }
        public string? Displacement { get; set; }
        public string? Model { get; set; }
        public string? Color { get; set; }
        public string? Price { get; set; }
        public string? Mileage { get; set; }
        public string? Location { get; set; }
        public string? FuelType { get; set; }
        public string? TransmissionType { get; set; }
        public string? OwnerContact { get; set; }
        public string? Condition { get; set; }
        public string? Extras { get; set; }
        public DateTime? SoatDate { get; set; }
        public DateTime? TechnicalInspection { get; set; }
        public string? Engine { get; set; }
        public string? Torque { get; set; }
        public string? Weight { get; set; }
        public string? Power { get; set; }
        public IFormFile? Image { get; set; }
        public DateTime? CreationDate { get; set; }
        public string? ClassificationDisplacement { get; set; }
        public int? BikeCategoryId { get; set; }
    }
}
