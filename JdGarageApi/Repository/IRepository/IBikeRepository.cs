using JdGarageApi.Models;

namespace JdGarageApi.Repository.IRepository
{
    public interface IBikeRepository : IVehicleRepository<Bike>
    {
        ICollection<Bike> GetBikesInBikeCategory(int bikeCategoryId); // Método nos permite obtener las motocicletas existentes en una categoría 
        IEnumerable<Bike> ShareBike(string bikeName); // Método que recibe el valor del modelo que se le envie por parámetro y retorna un resultado de búsqueda
    }
}
