using System.Net;
using JdGarageApi.Models;
using JdGarageApi.Models.DTOs;
using JdGarageApi.Repository.IRepository;
using Asp.Versioning;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JdGarageApi.Controllers
{
    [Route("api/v{version:apiVersion}/users")]
    [ApiController]
    [ApiVersion("1.0")]
    public class UsersController : ControllerBase
    {
        private readonly IUserRepository _usersRepository;
        private readonly IMapper _mapper;
        protected ResponseApi _responseApi;

        public UsersController(IUserRepository userRepository, IMapper mapper)
        {
            _usersRepository = userRepository;
            _mapper = mapper;
            this._responseApi = new();
        }

        //Obtener todos los usuarios
        [Authorize(Roles = "Administrador")]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult GetUsers()
        {
            var userList = _usersRepository.GetUsers();
            var userListDto = new List<UserDto>();

            foreach (var list in userList)
            {
                userListDto.Add(_mapper.Map<UserDto>(list));
            }
            return Ok(userListDto);
        }

        //Obtener cada usario de manera individual
        [Authorize(Roles = "Administrador")]
        [HttpGet("{userId}", Name = "GetUser")]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult GetUser(string userId)
        {
            var itemUser = _usersRepository.GetUser(userId);
            if (itemUser == null)
            {
                return NotFound();
            }

            var itemUserDto = _mapper.Map<UserDto>(itemUser);
            return Ok(itemUserDto);
        }

        //Registrar un usuario a través de la api
        [AllowAnonymous]
        [HttpPost("register")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Register([FromBody] UserRegisterDto userRegisterDto)
        {
            bool validateUniqueUserName = _usersRepository.IsUniqueUser(userRegisterDto.UserName);
            if (!validateUniqueUserName)
            {
                _responseApi.StatusCode = HttpStatusCode.BadRequest;
                _responseApi.IsSuccess = false;
                _responseApi.ErrorMessage.Add("El nombre de usuario ya existe");
                return BadRequest(_responseApi);
            }

            var user = await _usersRepository.Register(userRegisterDto);
            if (user == null)
            {
                _responseApi.StatusCode = HttpStatusCode.BadRequest;
                _responseApi.IsSuccess = false;
                _responseApi.ErrorMessage.Add("Error en el registro");
                return BadRequest(_responseApi);
            }

            _responseApi.StatusCode = HttpStatusCode.OK;
            _responseApi.IsSuccess = true;
            return Ok(_responseApi);
        }

        [AllowAnonymous]
        [HttpPost("login")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Login([FromBody] UserLoginDto userLoginDto)
        {
            var loginResponse = await _usersRepository.Login(userLoginDto);
            if (loginResponse.User == null || string.IsNullOrEmpty(loginResponse.Token))
            {
                _responseApi.StatusCode = HttpStatusCode.BadRequest;
                _responseApi.IsSuccess = false;
                _responseApi.ErrorMessage.Add("El nombre de usuario o password son incorrectos");
                return BadRequest(_responseApi);
            }
            
            _responseApi.StatusCode = HttpStatusCode.OK;
            _responseApi.IsSuccess = true;
            _responseApi.Result = loginResponse;
            return Ok(_responseApi);
        }
    }
}
