using AutoMapper;
using JdGarageApi.Models;
using JdGarageApi.Models.DTOs;
using JdGarageApi.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JdGarageApi.Controllers
{
    [Route("api/v{version:apiVersion}/brands")]
    [ApiController]
    [Authorize(Roles = "Administrador")]
    public class BrandsController : ControllerBase
    {
        private readonly IBrandRepository _brandRepository;
        private readonly IMapper _mapper;
        private readonly CloudinaryService _cloudinaryService;
        public BrandsController(IBrandRepository brandRepository, IMapper mapper, CloudinaryService cloudinaryService)
        {
            _brandRepository = brandRepository;
            _mapper = mapper;
            _cloudinaryService = cloudinaryService;
        }

        [AllowAnonymous]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetBrands([FromQuery] string brandType)
        {
            var brands = _brandRepository.GetBrands(brandType);
            if (brands == null || !brands.Any())
                return NotFound("No se encontraron marcas");

            var totalBrands = _brandRepository.GetTotalBrands(brandType);
            var brandsDto = _mapper.Map<IEnumerable<Brands>>(brands);

            var response = new { TotalBrands = totalBrands, Data = brandsDto };
            return Ok(response);
        }

        [AllowAnonymous]
        [HttpGet("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetBrandById(int id)
        {
            var brand = _brandRepository.GetBrand(id);
            if (brand == null)
                return NotFound();

            return Ok(_mapper.Map<Brands>(brand));
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> CreateBrand([FromForm] CreateBrandDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (_brandRepository.ExistBrand(dto.BrandName, dto.BrandType))
                return Conflict(new { message = $"La marca {dto.BrandName} ya existe" });

            var brand = _mapper.Map<Brands>(dto);

            if (dto.Image != null)
            {
                brand.ImageUrl = await _cloudinaryService.UploadBrandImageAsync(dto.Image);
            }

            _brandRepository.CreateBrand(brand);
            return CreatedAtAction(nameof(GetBrandById), new { id = brand.Id }, _mapper.Map<Brands>(brand));
        }

        [HttpPatch("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateBrand(int id, [FromForm] UpdateBrandDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (dto == null || id != dto.Id)
                return BadRequest();

            var brand = _brandRepository.GetBrand(id);
            if (brand == null)
                return NotFound();

            _mapper.Map(dto, brand);

            if (dto.Image != null)
            {
                brand.ImageUrl = await _cloudinaryService.UploadBrandImageAsync(dto.Image);
            }
            _brandRepository.UpdateBrand(brand);
            return Ok(_mapper.Map<Brands>(brand));
        }

        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult DeleteBrand(int id)
        {
            var brand = _brandRepository.GetBrand(id);
            if (brand == null)
                return NotFound();

            _brandRepository.DeleteBrand(brand);
            return NoContent();
        }
    }
}
