using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using JdGarageApi.Data;
using JdGarageApi.Models;
using JdGarageApi.Models.DTOs;
using JdGarageApi.Repository.IRepository;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using XSystem.Security.Cryptography;

namespace JdGarageApi.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _db;
        private string secretKey;
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IMapper _mapper;

        public UserRepository(ApplicationDbContext db, IConfiguration config, UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager, IMapper mapper)
        {
            _db = db;
            secretKey = config.GetValue<string>("ApiSettings:Secret");
            _userManager = userManager;
            _roleManager = roleManager;
            _mapper = mapper;
        }

        public AppUser GetUser (string userId)
        {
            return _db.AppUser.FirstOrDefault(x => x.Id == userId);
        }

        public ICollection<AppUser> GetUsers()
        {
            return _db.AppUser.OrderBy(x => x.UserName).ToList();
        }

        public bool IsUniqueUser(string user)
        {
            var userBd = _db.AppUser.FirstOrDefault(x =>x.UserName == user);
            if(userBd == null)
            {
                return true;
            }
            return false;
        }

        public async Task<UserLoginResponseDto> Login(UserLoginDto userLoginDto)
        {
            var user = _db.AppUser.FirstOrDefault(
                item => item.UserName.ToLower() == userLoginDto.UserName.ToLower());
            bool isValid = await _userManager.CheckPasswordAsync(user, userLoginDto.Password);

            //Validar si el usuario no existe con la combinación de usuario y contraseña
            if (user == null || isValid == false)
            {
                return new UserLoginResponseDto()
                {
                    Token = "",
                    User = null
                };
            }

            //Aqui si existe el usuario entonces podemos procesar el login
            var roles = await _userManager.GetRolesAsync(user);
            var tokenConfiguration = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(secretKey);


            //SecurityTokenDescriptor: Clase usada para escribir las propiedades del token
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.UserName.ToString()),
                    new Claim(ClaimTypes.Role, roles.FirstOrDefault())
                }),
                Expires = DateTime.UtcNow.AddDays(7),

                SigningCredentials = new(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature) //HmacSha256Signature-> Se especifica que se usará ese algoritmo (HmacSha256Signature) para la firma del token
            };

            var token = tokenConfiguration.CreateToken(tokenDescriptor);

            UserLoginResponseDto userLoginResponseDto = new UserLoginResponseDto()
            {
                Token = tokenConfiguration.WriteToken(token),
                User = _mapper.Map<UserDataDto>(user),
            };

            return userLoginResponseDto;
        }

        public async Task<UserDataDto> Register(UserRegisterDto userRegisterDto)
        {
            AppUser user = new AppUser()
            {
                UserName = userRegisterDto.UserName,
                Email = userRegisterDto.UserName,
                NormalizedEmail = userRegisterDto.UserName.ToUpper(),
                Name = userRegisterDto.Name
            };

            var result = await _userManager.CreateAsync(user, userRegisterDto.Password);
            if(result.Succeeded)
            {
                if(!_roleManager.RoleExistsAsync("Admin").GetAwaiter().GetResult())
                {
                    await _roleManager.CreateAsync(new IdentityRole("Admin"));
                    await _roleManager.CreateAsync(new IdentityRole("Registrado"));
                }

                await _userManager.AddToRoleAsync(user, "Admin");
                var userReturned = _db.AppUser.FirstOrDefault(item => item.UserName == userRegisterDto.UserName);
                return _mapper.Map<UserDataDto>(userReturned);
            }
            return new UserDataDto();
        }
    }
}
