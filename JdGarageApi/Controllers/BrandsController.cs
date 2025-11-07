using JdGarageApi.Models;
using JdGarageApi.Models.DTOs;
using JdGarageApi.Repository;
using JdGarageApi.Repository.IRepository;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JdGarageApi.Controllers
{
    [Route("api/v{version:apiVersion}/brands")]
    [Authorize(Roles = "Admin")]
    [ApiController]
    public class BrandsController : ControllerBase
    {
        private readonly IBrandRepository _brandRepository;
        private readonly IMapper _mapper;

        public BrandsController(IBrandRepository brandRepository, IMapper mapper)
        {
            _brandRepository = brandRepository;
            _mapper = mapper;
        }

        [AllowAnonymous]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult GetBrands([FromQuery] string brandType)
        {
            try
            {
                var totalBrands = _brandRepository.GetTotalBrands(brandType);
                var brands = _brandRepository.GetBrands(brandType);

                if (brands == null || !brands.Any())
                {
                    return NotFound("No se han encontrado marcas en esta sesión");
                }

                var brandsDto = brands.Select(item => _mapper.Map<BrandsDto>(item)).ToList();
                var response = new
                {
                    TotalBrands = totalBrands,
                    Data = brandsDto
                };
                return Ok(response);

            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error recuperando datos de la aplicación");
            }
        }

        [HttpPost]
        [ProducesResponseType(201, Type = typeof(BrandsDto))]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult CreateBrand([FromForm] CreateBrandDto createBrandDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (createBrandDto == null)
            {
                return BadRequest(ModelState);
            }
            if (_brandRepository.ExistBrand(createBrandDto.BrandName, createBrandDto.BrandType))
            {
                return Conflict(new{message = $"La marca {createBrandDto.BrandName} ya existe en la sección seleccionada"});
            }

            var brand = _mapper.Map<Brands>(createBrandDto);

            if (createBrandDto.Image != null)
            {
                string fileName = brand.Id + System.Guid.NewGuid().ToString() + Path.GetExtension(createBrandDto.Image.FileName);
                string fileRoute = @"wwwroot\BrandsImages\" + fileName;
                var directoryLocation = Path.Combine(Directory.GetCurrentDirectory(), fileRoute);
                FileInfo file = new FileInfo(directoryLocation);
                if (file.Exists)
                {
                    file.Delete();
                }
                using (var fileStream = new FileStream(directoryLocation, FileMode.Create))
                {
                    createBrandDto.Image.CopyTo(fileStream);
                }
                var baseUrl = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host.Value}{HttpContext.Request.PathBase.Value}";
                brand.UrlImage = baseUrl + "/BrandsImages/" + fileName;
                brand.UrlLocalImage = fileRoute;
            }
            else
            {
                brand.UrlImage = null;
            }

            _brandRepository.CreateBrand(brand);
            return Ok(brand);
        }

        [HttpPatch("{brandId:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]

        public IActionResult UpdatePatchBike(int brandId, [FromForm] UpdateBrandDto updateBrandDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (updateBrandDto == null || brandId != updateBrandDto.Id)
            {
                return BadRequest(ModelState);
            }

            var brandExist = (_brandRepository.GetBrand(brandId));
            var brand = _mapper.Map<Brands>(updateBrandDto);

            if (updateBrandDto.Image != null)
            {
                string fileName = brand.Id + System.Guid.NewGuid().ToString() + Path.GetExtension(updateBrandDto.Image.FileName);
                string fileRoute = @"wwwroot\BrandsImages\" + fileName;
                var directoryLocation = Path.Combine(Directory.GetCurrentDirectory(), fileRoute);
                FileInfo file = new FileInfo(directoryLocation);
                if (file.Exists)
                {
                    file.Delete();
                }
                using (var fileStream = new FileStream(directoryLocation, FileMode.Create))
                {
                    updateBrandDto.Image.CopyTo(fileStream);
                }
                var baseUrl = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host.Value}{HttpContext.Request.PathBase.Value}";
                brand.UrlImage = baseUrl + "/BrandsImages/" + fileName;
                brand.UrlLocalImage = fileRoute;
            }
            else
            {
                brand.UrlImage = brandExist.UrlImage;
                brand.UrlLocalImage = brandExist.UrlLocalImage;
            }

            _brandRepository.UpdateBrand(brand);
            return Ok(brand);

        }

        [HttpDelete("{brandId:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult DeleteBrand(int brandId)
        {

            var brand = _brandRepository.GetBrand(brandId);

            if (!_brandRepository.DeleteBrand(brand))
            {
                ModelState.AddModelError("", $"Algo salió mal borrando el registro  {brand.BrandName}");
                return StatusCode(500, ModelState);
            }
            return NoContent();
        }
    }
}
