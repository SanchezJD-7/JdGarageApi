using JdGarageApi.Data;
using JdGarageApi.Models;
using JdGarageApi.Repository.IRepository;
using JdGarageApi.Services;
using Microsoft.EntityFrameworkCore;

namespace JdGarageApi.Repository
{
    public class BikeRepository : VehicleRepository<Bike>, IBikeRepository
    {
        public BikeRepository(ApplicationDbContext db, IKpiService kpiService) : base(db, kpiService) { }

        public ICollection<Bike> GetBikesInBikeCategory(int bikeCategoryId)
        {
            return _db.Set<Bike>()
                .Include(bCategory => bCategory.BikeCategory)
                .Where(bCategory => bCategory.BikeCategoryId == bikeCategoryId)
                .ToList();
        }

        public IEnumerable<Bike> ShareBike(string bikeName)
        {
            IQueryable<Bike> query = _db.Set<Bike>();
            if (!string.IsNullOrEmpty(bikeName))
            {
                query = query.Where(item => item.Line.Contains(bikeName) || item.Brand.Contains(bikeName));
            }

            return query.ToList();
        }
    }
}
