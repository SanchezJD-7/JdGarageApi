using Asp.Versioning;
using AutoMapper;
using JdGarageApi.Models;
using JdGarageApi.Models.DTOs;
using JdGarageApi.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JdGarageApi.Controllers
{
    [Route("api/v{version:apiVersion}/bikes")]
    [ApiController]
    [ApiVersion("1.0")]
    [Authorize(Roles = "Administrador")]
    public class BikesController : ControllerBase
    {
        private readonly IBikeRepository _bikeRepository;
        private readonly IMapper _mapper;
        private readonly CloudinaryService _cloudinaryService;

        public BikesController(IBikeRepository bikeRepository, IMapper mapper, CloudinaryService cloudinaryService)
        {
            _bikeRepository = bikeRepository;
            _mapper = mapper;
            _cloudinaryService = cloudinaryService;
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult GetBikes(int pageNumber = 1, int pageSize = 10)
        {
            var total = _bikeRepository.GetTotalBikes();
            var bikes = _bikeRepository.GetBikes(pageNumber, pageSize);

            if (!bikes.Any())
                return NotFound();

            return Ok(new
            {
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalItems = total,
                TotalPages = (int)Math.Ceiling(total / (double)pageSize),
                Items = _mapper.Map<IEnumerable<BikeDto>>(bikes)
            });
        }

        [AllowAnonymous]
        [HttpGet("{bikeId:int}", Name = "GetBike")]
        public IActionResult GetBike(int bikeId)
        {
            var bike = _bikeRepository.GetBike(bikeId);
            if (bike == null)
                return NotFound();

            return Ok(_mapper.Map<BikeDto>(bike));
        }

        [HttpPost]
        public async Task<IActionResult> CreateBike([FromForm] CreateBikeDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var bike = _mapper.Map<Bike>(dto);

            if (dto.Image != null)
            {
                bike.ImageUrl = await _cloudinaryService.UploadBikeImageAsync(dto.Image);
            }

            _bikeRepository.CreateBike(bike);

            return CreatedAtRoute("GetBike", new { bikeId = bike.Id }, _mapper.Map<BikeDto>(bike));
        }

        [HttpPatch("{bikeId:int}")]
        public async Task<IActionResult> UpdateBike(
            int bikeId,
            [FromForm] UpdateBikeDto dto)
        {
            if (!ModelState.IsValid || dto == null || bikeId != dto.Id)
                return BadRequest();

            var bike = _bikeRepository.GetBike(bikeId);
            if (bike == null)
                return NotFound();

            _mapper.Map(dto, bike);

            if (dto.Image != null)
            {
                bike.ImageUrl = await _cloudinaryService.UploadBikeImageAsync(dto.Image);
            }
            _bikeRepository.UpdateBike(bike);
            return Ok(_mapper.Map<BikeDto>(bike));
        }

        [HttpDelete("{bikeId:int}")]
        public IActionResult DeleteBike(int bikeId)
        {
            if (!_bikeRepository.ExistBike(bikeId))
                return NotFound();

            var bike = _bikeRepository.GetBike(bikeId);
            _bikeRepository.DeleteBike(bike);

            return NoContent();
        }

        [AllowAnonymous]
        [HttpGet("Brand")]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult GetBikesByBrand([FromQuery] string brand)
        {
            try
            {
                var bikesResponse = _bikeRepository.GetBikesByBrand(brand);
                return Ok(bikesResponse);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error recuperando motocicletas de la marca");
            }
        }
    }
}
