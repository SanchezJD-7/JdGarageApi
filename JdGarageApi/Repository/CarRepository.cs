using JdGarageApi.Data;
using JdGarageApi.Models;
using JdGarageApi.Repository.IRepository;
using JdGarageApi.Services;

namespace JdGarageApi.Repository
{
    public class CarRepository : VehicleRepository<Car>, ICarRepository
    {
        public CarRepository(ApplicationDbContext db, IKpiService kpiService) : base(db, kpiService) { }
    }
}
