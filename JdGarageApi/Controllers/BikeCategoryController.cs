using System.Collections.Generic;
using JdGarageApi.Models;
using JdGarageApi.Models.DTOs;
using JdGarageApi.Repository.IRepository;
using Asp.Versioning;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace JdGarageApi.Controllers
{
    [Route("api/v{version:apiVersion}/BikeCategory")]
    [ApiController]
    [ApiVersion("1.0")]
    [ApiVersion("2.0")]
    public class BikeCategoryController : ControllerBase
    {
        private readonly IBikeCategoryRepository _bCatRepository;
        private readonly IMapper _mapper;

        public BikeCategoryController(IBikeCategoryRepository bCatRepository, IMapper mapper)
        {
            _bCatRepository = bCatRepository;
            _mapper = mapper;
        }

        //Obtener todas las categorías
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult GetBikesCategory() 
        {
            var bikeCategoryList = _bCatRepository.GetCategories();
            var bikeCategoryListDto = new List<BikeCategoryDto>();

            foreach (var list in bikeCategoryList) { 
                bikeCategoryListDto.Add(_mapper.Map<BikeCategoryDto>(list));
            }
            return Ok(bikeCategoryListDto);
        }

        //Obtener todas la categoría individual
        [HttpGet("{bikeCategoryId:int}", Name = "GetBikeCategory")]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult GetBikeCategory(int bikeCategoryId)
        {
            var itemBikeCategory = _bCatRepository.GetCategory(bikeCategoryId);
            if (itemBikeCategory == null) {
                return NotFound();
            }

            var itemBikeCategoryDto = _mapper.Map<BikeCategoryDto>(itemBikeCategory);
            return Ok(itemBikeCategoryDto);
        }

        //Crear nueva categoría
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult CreateBikeCategory([FromBody] CreateBikeCategoryDto createBikeCategoryDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (createBikeCategoryDto == null)
            {
                return BadRequest(ModelState);
            }
            if (_bCatRepository.ExistCategory(createBikeCategoryDto.CategoryName))
            {
                ModelState.AddModelError("", $"La categoría ya existe");
                return StatusCode(404, ModelState);
            }

            var bikeCategory = _mapper.Map<BikeCategory>(createBikeCategoryDto);

            if (!_bCatRepository.CreateCategory(bikeCategory)) {
                ModelState.AddModelError("", $"Algo salió mal guardando el registro {bikeCategory.CategoryName}");
                return StatusCode(404, ModelState);
            }

            return CreatedAtRoute("GetBikeCategory", new {bikeCategoryId = bikeCategory.Id }, bikeCategory);
        }

        //Actualizar el campo de una categoría
        [Authorize(Roles = "Admin")]
        [HttpPatch ("{bikeCategoryId:int}", Name = "UpdatePatchBikeCategory")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]

        public IActionResult UpdatePatchBikeCategory(int bikeCategoryId, [FromBody] BikeCategoryDto bikeCategoryDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (bikeCategoryDto == null || bikeCategoryId != bikeCategoryDto.Id)
            {
                return BadRequest(ModelState);
            }

            var bikeCategoryExist = (_bCatRepository.GetCategory(bikeCategoryId));

            if (bikeCategoryExist == null)
            {
                return NotFound($"No se encontró la categoría con ID {bikeCategoryId}");
            }

            var bikeCategory = _mapper.Map<BikeCategory>(bikeCategoryDto);

            if (!_bCatRepository.UpdateCategory(bikeCategory))
            {
                ModelState.AddModelError("", $"Algo salió mal actualizando el registro {bikeCategory.CategoryName}");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }

        //Actualizar una categoría
        [Authorize(Roles = "Admin")]
        [HttpPut("{bikeCategoryId:int}", Name = "UpdatePuthBikeCategory")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult UpdatePuthBikeCategory(int bikeCategoryId, [FromBody] BikeCategoryDto bikeCategoryDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (bikeCategoryDto == null || bikeCategoryId != bikeCategoryDto.Id)
            {
                return BadRequest(ModelState);
            }

            var bikeCategoryExist = (_bCatRepository.GetCategory(bikeCategoryId));
            
            if (bikeCategoryExist == null)
            {
                return NotFound($"No se encontró la categoría con ID {bikeCategoryId}");
            }

            var bikeCategory = _mapper.Map<BikeCategory>(bikeCategoryDto);

            if (!_bCatRepository.UpdateCategory(bikeCategory))
            {
                ModelState.AddModelError("", $"Algo salió mal actualizando el registro {bikeCategory.CategoryName}");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }

        //Eliminar una categoría
        [Authorize(Roles = "Admin")]
        [HttpDelete("{bikeCategoryId:int}", Name = "DeleteBikeCategory")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult DeleteBikeCategory(int bikeCategoryId)
        {

            if (!_bCatRepository.ExistCategory(bikeCategoryId))
            {
                return NotFound();
            }

            var bikeCategory = _bCatRepository.GetCategory(bikeCategoryId);

            if (!_bCatRepository.DeleteCategory(bikeCategory))
            {
                ModelState.AddModelError("", $"Algo salió mal borrando el registro {bikeCategory.CategoryName}");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }
    }
}
