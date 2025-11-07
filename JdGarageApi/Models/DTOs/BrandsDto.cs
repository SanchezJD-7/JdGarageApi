
namespace JdGarageApi.Models.DTOs
{
    public class BrandsDto
    {
        public int Id { get; set; }
        public string BrandName { get; set; }
        public string BrandType { get; set; }
        public string UrlImage { get; set; }
        public DateTime? CreationDate { get; set; }
        public DateTime? LastUpdateDate { get; set; }
    }
}
