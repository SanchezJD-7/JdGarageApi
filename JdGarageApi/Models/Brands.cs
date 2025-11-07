using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JdGarageApi.Models
{
    public class Brands
    {
        [Key]
        public int Id { get; set; }
        public string BrandName { get; set; }
        public string BrandType { get; set; }
        public string UrlImage { get; set; }
        public string UrlLocalImage { get; set; }
        public DateTime? CreationDate { get; set; }
        public DateTime? LastUpdateDate { get; set; }
    }
}
