using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JdGarageApi.Models
{
    public class Brands
    {
        public int Id { get; set; }
        public string BrandName { get; set; }
        public string BrandType { get; set; }
        public string ImageUrl { get; set; }
        public DateTime? CreationDate { get; set; }
        public DateTime? LastUpdateDate { get; set; }
    }

}
