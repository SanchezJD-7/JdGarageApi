namespace JdGarageApi.Models.DTOs
{
    public class CreateBikeDto
    {
        public string Brand { get; set; }
        public string Line { get; set; }
        public string Displacement { get; set; }
        public string Model { get; set; }
        public string Color { get; set; }
        public string? UrlImage { get; set; }
        public IFormFile Image { get; set; }
        public string ClassificationDisplacement { get; set; }
        public int BikeCategoryId { get; set; }
    }
}
