using JdGarageApi.Data;
using JdGarageApi.Models;
using JdGarageApi.Repository.IRepository;
using Microsoft.EntityFrameworkCore;

namespace JdGarageApi.Repository
{
    public class VehicleRepository<TVehicle> : IVehicleRepository<TVehicle> where TVehicle : Vehicle
    {
        protected readonly ApplicationDbContext _db;

        public VehicleRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public virtual bool Create(TVehicle vehicle)
        {
            vehicle.CreationDate = DateTime.Now;
            _db.Set<TVehicle>().Add(vehicle);
            return Save();
        }

        public virtual bool Delete(TVehicle vehicle)
        {
            _db.Set<TVehicle>().Remove(vehicle);
            return Save();
        }

        public virtual bool Exists(int id)
        {
            return _db.Set<TVehicle>().Any(x => x.Id == id);
        }

        public virtual bool Exists(string name)
        {
            return _db.Set<TVehicle>().Any(x => x.Line.ToLower().Trim() == name.ToLower().Trim());
        }

        public virtual ICollection<TVehicle> GetAll(int pageNumber, int pageSize)
        {
            return _db.Set<TVehicle>()
                .OrderBy(x => x.Line)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();
        }

        public virtual int GetTotal()
        {
            return _db.Set<TVehicle>().Count();
        }

        public virtual TVehicle GetById(int id)
        {
            return _db.Set<TVehicle>().FirstOrDefault(x => x.Id == id);
        }

        public virtual bool Save()
        {
            return _db.SaveChanges() >= 0;
        }

        public virtual bool Update(TVehicle vehicle)
        {
            vehicle.CreationDate = DateTime.Now;
            var existing = _db.Set<TVehicle>().Find(vehicle.Id);
            if (existing != null)
            {
                _db.Entry(existing).CurrentValues.SetValues(vehicle);
            }
            else
            {
                _db.Set<TVehicle>().Update(vehicle);
            }
            return Save();
        }

        public virtual ICollection<TVehicle> GetByBrand(string brand)
        {
            return _db.Set<TVehicle>().Where(item => item.Brand == brand).ToList();
        }
    }
}
