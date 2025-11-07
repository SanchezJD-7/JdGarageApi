namespace JdGarageApi.Models.DTOs
{
    public class CreateBrandDto
    {
        public string BrandName { get; set; }
        public string BrandType { get; set; }
        public string UrlImage { get; set; }
        public IFormFile Image { get; set; }
        public DateTime CreationDate { get; set; }
    }
}
