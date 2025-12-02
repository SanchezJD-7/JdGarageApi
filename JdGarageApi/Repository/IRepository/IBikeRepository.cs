using JdGarageApi.Models;

namespace JdGarageApi.Repository.IRepository
{
    public interface IBikeRepository
    {
        ICollection<Bike> GetBikes(int pageNumber, int pageSize); //Método para retornar las motocicletas por página
        ICollection<Bike> GetBikesInBikeCategory(int bikeCategoryId); // Método nos permite obtener las motocicletas existentes en una categoría 
        ICollection<Bike> GetBikesByBranch(string brand); // Método nos permite obtener las motocicletas existentes de una misma marca
        IEnumerable<Bike> ShareBike(string bikeName); // Método que recibe el valor del modelo que se le envie por parámetro y retorna un resultado de búsqueda
        Bike GetBike(int bikeId); //Método que nos retorna las motocicletas por ID
        bool ExistBike(int bikeId); //Método que valida por ID si la motocicleta existe
        bool ExistBike(string bikeName); //Método que valida por NOMBRE si la motocicleta existe
        bool CreateBike(Bike bike); //Método que recibe el modelo de la motocicleta para crear una nueva 
        bool UpdateBike(Bike bike); //Método que recibe el modelo de la motocicleta para actualizarla
        bool DeleteBike(Bike bike); //Método que recibe el modelo de la motocicleta para eliminarla
        bool Save();
        int GetTotalBikes(); //Trae la totalidad de registros
    }
}
