using Asp.Versioning;
using AutoMapper;
using JdGarageApi.Models;
using JdGarageApi.Models.DTOs;
using JdGarageApi.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JdGarageApi.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Authorize(Roles = "Administrador")]
    public abstract class VehiclesController<TVehicle, TVehicleDto, TCreateDto, TUpdateDto> : ControllerBase
        where TVehicle : Vehicle
        where TVehicleDto : VehicleDto
        where TCreateDto : CreateVehicleDto
        where TUpdateDto : UpdateVehicleDto
    {
        private readonly IVehicleRepository<TVehicle> _repository;
        private readonly IBrandRepository _brandRepository;
        private readonly IMapper _mapper;
        private readonly CloudinaryService _cloudinaryService;
        private readonly string _brandType;
        private readonly string _entityNamePlural;
        private readonly Func<IFormFile, Task<string>> _uploadImageAsync;

        protected VehiclesController(
            IVehicleRepository<TVehicle> repository,
            IBrandRepository brandRepository,
            IMapper mapper,
            CloudinaryService cloudinaryService,
            string brandType,
            string entityNamePlural,
            Func<IFormFile, Task<string>> uploadImageAsync)
        {
            _repository = repository;
            _brandRepository = brandRepository;
            _mapper = mapper;
            _cloudinaryService = cloudinaryService;
            _brandType = brandType;
            _entityNamePlural = entityNamePlural;
            _uploadImageAsync = uploadImageAsync;
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult GetAll(int pageNumber = 1, int pageSize = 10)
        {
            var total = _repository.GetTotal();
            var items = _repository.GetAll(pageNumber, pageSize);

            if (!items.Any())
                return NotFound();

            return Ok(new
            {
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalItems = total,
                TotalPages = (int)Math.Ceiling(total / (double)pageSize),
                Items = _mapper.Map<IEnumerable<TVehicleDto>>(items)
            });
        }

        [AllowAnonymous]
        [HttpGet("{id:int}")]
        public IActionResult GetById(int id)
        {
            var item = _repository.GetById(id);
            if (item == null)
                return NotFound();

            return Ok(_mapper.Map<TVehicleDto>(item));
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromForm] TCreateDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var vehicle = _mapper.Map<TVehicle>(dto);

            if (dto.Image != null)
            {
                vehicle.ImageUrl = await _uploadImageAsync(dto.Image);
            }

            _repository.Create(vehicle);

            return CreatedAtAction(nameof(GetById), new { id = vehicle.Id }, _mapper.Map<TVehicleDto>(vehicle));
        }

        [HttpPatch("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromForm] TUpdateDto dto)
        {
            if (!ModelState.IsValid || dto == null || id != dto.Id)
                return BadRequest();

            var vehicle = _repository.GetById(id);
            if (vehicle == null)
                return NotFound();

            _mapper.Map(dto, vehicle);

            if (dto.Image != null)
            {
                vehicle.ImageUrl = await _uploadImageAsync(dto.Image);
            }
            _repository.Update(vehicle);
            return Ok(_mapper.Map<TVehicleDto>(vehicle));
        }

        [HttpDelete("{id:int}")]
        public IActionResult Delete(int id)
        {
            if (!_repository.Exists(id))
                return NotFound();

            var vehicle = _repository.GetById(id);
            _repository.Delete(vehicle);

            return NoContent();
        }

        [AllowAnonymous]
        [HttpGet("Brand")]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetByBrand([FromQuery] string brand)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(brand))
                    return BadRequest("El parámetro 'brand' es requerido");

                if (!_brandRepository.ExistBrand(brand, _brandType))
                    return NotFound($"La marca '{brand}' no existe en el catálogo de marcas para {_entityNamePlural}");

                var items = _repository.GetByBrand(brand);
                return Ok(items);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error recuperando {_entityNamePlural} de la marca");
            }
        }
    }
}
