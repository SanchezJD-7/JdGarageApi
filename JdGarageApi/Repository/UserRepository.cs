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
using Microsoft.EntityFrameworkCore;

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

        public async Task<UserLoginResponseDto> Login(UserLoginDto dto)
        {
            AppUser user;
            if (dto.Identifier.Contains("@"))
                user = await _userManager.FindByEmailAsync(dto.Identifier);
            else
                user = await _userManager.FindByNameAsync(dto.Identifier);

            if (user == null)
                return new UserLoginResponseDto { Token = "", User = null, Roles = new() };

            var isValid = await _userManager.CheckPasswordAsync(user, dto.Password);
            if (!isValid)
                return new UserLoginResponseDto { Token = "", User = null, Roles = new() };

            var roles = await _userManager.GetRolesAsync(user);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Name, user.Name),
                new Claim("username", user.UserName),
                new Claim(ClaimTypes.Email, user.Email)
            };

            claims.AddRange(roles.Select(r => new Claim(ClaimTypes.Role, r)));

            var key = Encoding.ASCII.GetBytes(secretKey);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddHours(6),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature)
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return new UserLoginResponseDto
            {
                Token = tokenHandler.WriteToken(token),
                User = _mapper.Map<UserDataDto>(user),
                Roles = roles.ToList()
            };
        }

        public async Task<UserDataDto> Register(UserRegisterDto dto)
        {
            if (await _userManager.FindByNameAsync(dto.UserName) != null)
                throw new Exception("El nombre de usuario ya existe");

            var documentExists = await _userManager.Users.AnyAsync(u => u.DocumentNumber == dto.DocumentNumber);
            if (documentExists)
                throw new Exception("El número de documento ya está registrado");

            var fullName = $"{dto.FirstName.Trim()} {dto.LastName.Trim()}";

            var user = new AppUser
            {
                UserName = dto.UserName.Trim(),
                Email = dto.Email.Trim(),
                PhoneNumber = dto.PhoneNumber,
                FirstName = dto.FirstName.Trim(),
                LastName = dto.LastName.Trim(),
                Name = fullName,
                DocumentType = dto.DocumentType?.ToUpper(),
                DocumentNumber = dto.DocumentNumber?.Trim(),
                BirthDate = dto.BirthDate,
                State = dto.State,
                City = dto.City,
                Address = dto.Address
            };

            var result = await _userManager.CreateAsync(user, dto.Password);

            if (!result.Succeeded)
            {
                var errors = string.Join(" | ", result.Errors.Select(e => e.Description));
                throw new Exception(errors);
            }

            if (dto.RequestAdminRole)
            {
                const string adminRole = "Administrador";
                if (!await _roleManager.RoleExistsAsync(adminRole))
                    await _roleManager.CreateAsync(new IdentityRole(adminRole));
                await _userManager.AddToRoleAsync(user, adminRole);
            }
            return _mapper.Map<UserDataDto>(user);
        }
    }
}
