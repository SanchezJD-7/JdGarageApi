using System.ComponentModel.DataAnnotations;

namespace JdGarageApi.Models
{
    public class Kpi
    {
        public int Users { get; set; }
        public int Vehicles { get; set; }
        public int Bikes { get; set; }
        public int Cars { get; set; }
    }
}
