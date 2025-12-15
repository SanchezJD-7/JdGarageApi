
namespace JdGarageApi.Models.DTOs
{
    public class UpdateBrandDto
    {
        public int Id { get; set; }
        public string BrandName { get; set; }
        public string BrandType { get; set; }
        public IFormFile Image { get; set; }
        public DateTime LastUpdateDate { get; set; }
    }
}

