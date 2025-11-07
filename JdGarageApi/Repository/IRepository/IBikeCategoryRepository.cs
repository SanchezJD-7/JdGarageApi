using JdGarageApi.Models;

namespace JdGarageApi.Repository.IRepository
{
    public interface IBikeCategoryRepository
    {
        ICollection<BikeCategory> GetCategories(); //Método que nos trae todas las categorías
        BikeCategory GetCategory(int BikeCategoryId); //Método que nos retorna las categorías por ID
        bool ExistCategory(int BikeCategoryId); //Método que valida por ID si la categoría existe
        bool ExistCategory(string CategoryName); //Método que valida por NOMBRE si la categoría existe
        bool CreateCategory(BikeCategory BikeCategory); //Método que recibe el modelo de la categoría para crear una nueva categoría
        bool UpdateCategory(BikeCategory BikeCategory); //Método que recibe el modelo de la categoría para actualizar una categoría
        bool DeleteCategory(BikeCategory BikeCategory); //Método que recibe el modelo de la categoría para eliminar una categoría
        bool Save();
    }
}
