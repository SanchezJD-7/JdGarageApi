using JdGarageApi.Models;
using JdGarageApi.Models.DTOs;
using JdGarageApi.Repository.IRepository;
using Asp.Versioning;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JdGarageApi.Controllers
{
    [Route("api/v{version:apiVersion}/bikes")]
    [Authorize(Roles = "Admin")]
    [ApiController]
    [ApiVersion("1.0")]
    public class BikesController : ControllerBase
    {
        private readonly IBikeRepository _bikeRepository;
        private readonly IMapper _mapper;

        public BikesController(IBikeRepository bikeRepository, IMapper mapper)
        {
            _bikeRepository = bikeRepository;
            _mapper = mapper;
        }
        //Obtener todas las motocicletas
        [AllowAnonymous] //Esto se usa para permitir que cualquier persona tenga acceso a esta función
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult GetBikes([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            try
            {
                var totalBikes = _bikeRepository.GetTotalBikes();
                var bikes = _bikeRepository.GetBikes(pageNumber, pageSize);

                if (bikes == null || !bikes.Any())
                {
                    return NotFound("No se encontraron mototcicletas");
                }

                var bikesDto = bikes.Select(item => _mapper.Map<BikeDto>(item)).ToList();
                var response = new
                {
                    PageNumber = pageNumber,
                    PageSize = pageSize,
                    TotalPages = (int)Math.Ceiling(totalBikes / (double)pageSize),
                    TotalItems = totalBikes,
                    Items = bikesDto
                };
                return Ok(response);

            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error recuperando datos de la aplicación");
            }
        }

        //Obtener cada motocicleta de manera individual
        [AllowAnonymous]
        [HttpGet("{bikeId:int}", Name = "GetBike")]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult GetBike(int bikeId)
        {
            var itemBike = _bikeRepository.GetBike(bikeId);
            if (itemBike == null)
            {
                return NotFound();
            }

            var itemBikeDto = _mapper.Map<BikeDto>(itemBike);
            return Ok(itemBikeDto);
        }

        //Ingresar nueva motocicleta
        [HttpPost]
        [ProducesResponseType(201, Type = typeof(BikeDto))]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult CreateBike([FromForm] CreateBikeDto createBikeDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (createBikeDto == null)
            {
                return BadRequest(ModelState);
            }

            var bike = _mapper.Map<Bike>(createBikeDto);

            //Subida de archivo al subir una motocicleta
            if (createBikeDto.Image != null)
            {
                string fileName = bike.Id + System.Guid.NewGuid().ToString() + Path.GetExtension(createBikeDto.Image.FileName);
                string fileRoute = @"wwwroot\VehicleImages\" + fileName;
                var directoryLocation = Path.Combine(Directory.GetCurrentDirectory(), fileRoute);
                FileInfo file = new FileInfo(directoryLocation);
                if (file.Exists)
                {
                    file.Delete();
                }
                using (var fileStream = new FileStream(directoryLocation, FileMode.Create))
                {
                    createBikeDto.Image.CopyTo(fileStream);
                }
                var baseUrl = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host.Value}{HttpContext.Request.PathBase.Value}";
                bike.UrlImage = baseUrl + "/VehicleImages/" + fileName;
                bike.UrlLocalImage = fileRoute;
            }
            else
            {
                bike.UrlImage = null;
            }

            _bikeRepository.CreateBike(bike);
            return CreatedAtRoute("GetBike", new { bikeId = bike.Id }, bike);
        }

        //Actualizar el campo de una motocicleta
        [HttpPatch("{bikeId:int}", Name = "UpdatePatchBike")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]

        public IActionResult UpdatePatchBike(int bikeId, [FromForm] UpdateBikeDto updateBikeDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (updateBikeDto == null || bikeId != updateBikeDto.Id)
            {
                return BadRequest(ModelState);
            }

            var bikeExist = (_bikeRepository.GetBike(bikeId));

            if (bikeExist == null)
            {
                return NotFound($"No se encontró la motocicleta con ID {bikeId}");
            }

            var bike = _mapper.Map(updateBikeDto, bikeExist);

            //Actualizar archivo
            if (updateBikeDto.Image != null)
            {
                string fileName = bike.Id + System.Guid.NewGuid().ToString() + Path.GetExtension(updateBikeDto.Image.FileName);
                string fileRoute = @"wwwroot\VehicleImages\" + fileName;
                var directoryLocation = Path.Combine(Directory.GetCurrentDirectory(), fileRoute);
                FileInfo file = new FileInfo(directoryLocation);
                if (file.Exists)
                {
                    file.Delete();
                }
                using (var fileStream = new FileStream(directoryLocation, FileMode.Create))
                {
                    updateBikeDto.Image.CopyTo(fileStream);
                }
                var baseUrl = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host.Value}{HttpContext.Request.PathBase.Value}";
                bike.UrlImage = baseUrl + "/VehicleImages/" + fileName;
                bike.UrlLocalImage = fileRoute;
            }
            else
            {
                bike.UrlImage = bikeExist.UrlImage;
                bike.UrlLocalImage = bikeExist.UrlLocalImage;
            }

            _bikeRepository.UpdateBike(bike);
            return Ok(bike);

        }

        //Eliminar una motocicleta
        [HttpDelete("{bikeId:int}", Name = "DeleteBike")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult DeleteBike(int bikeId)
        {

            if (!_bikeRepository.ExistBike(bikeId))
            {
                return NotFound();
            }

            var bike = _bikeRepository.GetBike(bikeId);

            if (!_bikeRepository.DeleteBike(bike))
            {
                ModelState.AddModelError("", $"Algo salió mal borrando el registro  {bike.Brand} + {bike.Line}");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }

        //Consultar las motocicletas en una categoría
        [AllowAnonymous]
        [HttpGet("GetBikesInBikeCategory/{categoryId:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult GetBikesInBikeCategory(int categoryId)
        {
            try
            {
                var bikesList = _bikeRepository.GetBikesInBikeCategory(categoryId);
                if (bikesList == null || !bikesList.Any())
                {
                    return NotFound($"No se encontraron motocicletas en la categoría con ID {categoryId}.");
                }

                var bikeItem = bikesList.Select(bike => _mapper.Map<BikeDto>(bike)).ToList();
                return Ok(bikeItem);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Ocurrió un error recuperando datos de la aplicación");
            }
        }

        //Buscar una motocicleta en buscador
        [AllowAnonymous]
        [HttpGet("ShareBike")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult ShareBike(string bike)
        {
            try
            {
                var bikes = _bikeRepository.ShareBike(bike);
                if (!bikes.Any())
                {
                    return NotFound($"No se encontraron motocicletas que coincidan con la búsqueda.");
                }
                var bikesDto = _mapper.Map<IEnumerable<BikeDto>>(bikes);
                return Ok(bikesDto);

            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error recuperando datos de la aplicación");
            }

        }

        [AllowAnonymous]
        [HttpGet("Brand")]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult GetBikesByBranch([FromQuery] string brand)
        {
            try
            {
                var bikesResponse = _bikeRepository.GetBikesByBranch(brand);
                return Ok(bikesResponse);

            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error recuperando motocicletas de la marca");
            }
        }

    }
}
