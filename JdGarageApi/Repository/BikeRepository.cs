using JdGarageApi.Data;
using JdGarageApi.Models;
using JdGarageApi.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace JdGarageApi.Repository
{
    public class BikeRepository : IBikeRepository
    {
        private readonly ApplicationDbContext _db;

        public BikeRepository(ApplicationDbContext db)
        {
            _db = db;
        }
        
        public bool CreateBike(Bike bike)
        {
            bike.CreationDate = DateTime.Now;
            _db.Bike.Add(bike);
            return Save();
        }

        public bool DeleteBike(Bike bike)
        {
            _db.Bike.Remove(bike);
            return Save();
        }

        public bool ExistBike(int bikeId)
        {
            return _db.Bike.Any(x => x.Id == bikeId);
        }

        public bool ExistBike(string bikeName)
        {
            bool name = _db.Bike.Any(x => x.Line.ToLower().Trim() == bikeName.ToLower().Trim());
            return name;
        }

        public ICollection<Bike> GetBikes(int pageNumber, int pageSize)
        {
            return _db.Bike.OrderBy(x => x.Line)
                .Skip((pageNumber -1) * pageSize)
                .Take(pageSize)
                .ToList();
        }

        public int GetTotalBikes()
        {
            return _db.Bike.Count();
        }

        public Bike GetBike(int bikeId)
        {
            return _db.Bike.FirstOrDefault (x => x.Id == bikeId);
        }

        public bool Save()
        {
            return _db.SaveChanges() >= 0 ? true : false;
        }

        public bool UpdateBike(Bike bike)
        {
            bike.CreationDate = DateTime.Now;
            var bikeExist = _db.Bike.Find(bike.Id);
            if (bikeExist != null)
            {
                _db.Entry(bikeExist).CurrentValues.SetValues(bike);
            }
            else
            {
                _db.Bike.Update(bike);
            }
            return Save();
        }

        public ICollection<Bike> GetBikesInBikeCategory(int bikeCategoryId)
        {
            return _db.Bike.Include(bCategory => bCategory.BikeCategory).Where(bCategory => bCategory.BikeCategoryId == bikeCategoryId).ToList();
        }

        public IEnumerable<Bike> ShareBike(string bikeName)
        {
            IQueryable<Bike> query = _db.Bike;
            if (!string.IsNullOrEmpty(bikeName))
            {
                query = query.Where(item => item.Line.Contains(bikeName) || item.Brand.Contains(bikeName));
            }

            return query.ToList();
        }
    }
}
