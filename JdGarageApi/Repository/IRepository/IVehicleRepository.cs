using JdGarageApi.Models;

namespace JdGarageApi.Repository.IRepository
{
    public interface IVehicleRepository<TVehicle> where TVehicle : Vehicle
    {
        ICollection<TVehicle> GetAll(int pageNumber, int pageSize);
        TVehicle GetById(int id);
        ICollection<TVehicle> GetByBrand(string brand);
        bool Exists(int id);
        bool Exists(string name);
        bool Create(TVehicle vehicle);
        bool Update(TVehicle vehicle);
        bool Delete(TVehicle vehicle);
        bool Save();
        int GetTotal();
    }
}
