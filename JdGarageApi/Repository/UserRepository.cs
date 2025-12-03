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

        public AppUser GetUser(string userId)
        {
            return _userManager.Users.FirstOrDefault(x => x.Id == userId);
        }


        public ICollection<AppUser> GetUsers()
        {
            return _userManager.Users.OrderBy(x => x.UserName).ToList();

        }

        public bool IsUniqueUser(string user)
        {
            var userBd = _userManager.Users.FirstOrDefault(x => x.UserName == user);

            if (userBd == null)
            {
                return true;
            }
            return false;
        }

        public async Task<UserLoginResponseDto> Login(UserLoginDto userLoginDto)
        {
            var user = await _userManager.FindByNameAsync(userLoginDto.UserName);
            if (user == null)
            {
                return new UserLoginResponseDto()
                {
                    Token = "",
                    User = null,
                    Roles = new List<string>()
                };
            }
            bool isValid = await _userManager.CheckPasswordAsync(user, userLoginDto.Password);

            if (!isValid)
            {
                return new UserLoginResponseDto()
                {
                    Token = "",
                    User = null,
                    Roles = new List<string>()
                };
            }

            var roles = await _userManager.GetRolesAsync(user);
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(secretKey);
            var identityClaims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName)
            };

            identityClaims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(identityClaims),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return new UserLoginResponseDto()
            {
                Token = tokenHandler.WriteToken(token),
                User = _mapper.Map<UserDataDto>(user),
                Roles = roles.ToList()
            };
        }

        public async Task<UserDataDto> Register(UserRegisterDto userRegisterDto)
        {
            AppUser user = new AppUser()
            {
                UserName = userRegisterDto.UserName,
                Email = userRegisterDto.UserName,
                Name = userRegisterDto.Name
            };

            var result = await _userManager.CreateAsync(user, userRegisterDto.Password);

            if (!result.Succeeded) return new UserDataDto();

            var validRoles = new List<string> { "Administrador", "Vendedor", "Comprador" };

            foreach (var role in validRoles)
            {
                if (!await _roleManager.RoleExistsAsync(role))
                    await _roleManager.CreateAsync(new IdentityRole(role));
            }

            if (userRegisterDto.Roles == null || userRegisterDto.Roles.Count == 0)
            {
                await _userManager.AddToRoleAsync(user, "Comprador");
            }
            else
            {
                foreach (var role in userRegisterDto.Roles)
                {
                    if (validRoles.Contains(role))
                        await _userManager.AddToRoleAsync(user, role);
                }
            }
            var createdUser = await _userManager.FindByNameAsync(user.UserName);
            return _mapper.Map<UserDataDto>(createdUser);
        }

    }
}
