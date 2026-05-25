using JdGarageApi.Data;
using JdGarageApi.Models;
using JdGarageApi.Repository.IRepository;

namespace JdGarageApi.Repository
{
    public class CarRepository : VehicleRepository<Car>, ICarRepository
    {
        public CarRepository(ApplicationDbContext db) : base(db) { }
    }
}
