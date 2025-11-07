using JdGarageApi.Models;
using JdGarageApi.Models.DTOs;

namespace JdGarageApi.Repository.IRepository
{
    public interface IUserRepository
    {

        ICollection<AppUser> GetUsers(); //Método que nos trae todos los usuarios
        AppUser GetUser(string userId); //Método que nos retorna un usuario por ID
        bool IsUniqueUser(string user); //Método que valida por nombre de usuario si el usuario existe o no para asi permitir crearlo
        Task<UserLoginResponseDto> Login(UserLoginDto userLoginDto); //Método para obtener respuesta del usuario una vez se autentique
        Task<UserDataDto> Register(UserRegisterDto userRegisterDto); //Método para registrar un usuario
    }
}
