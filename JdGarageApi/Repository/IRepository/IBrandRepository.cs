using JdGarageApi.Models;

namespace JdGarageApi.Repository.IRepository
{
    public interface IBrandRepository
    {
        ICollection<Brands> GetBrands(string brandType); //Retornamos todas las marcas dependiendo de si es de carros o motocicletas
        bool ExistBrand(string brandName, string brandType); //Método que valida por NOMBRE y por TIPO si la marca existe
        bool CreateBrand(Brands brand); //Método que recibe el modelo de la marca para crear una nueva
        bool UpdateBrand(Brands brand); //Método que recibe el modelo de la marca y la actualiza
        bool DeleteBrand(Brands brand); //Método que recibe el modelo de la marca y la elimina
        Brands GetBrand(int brandId); //Método que nos retorna la marca por ID
        bool Save();
        int GetTotalBrands(string brandType); //Trae la totalidad de marcas filteando por carro o moto
    }
}
