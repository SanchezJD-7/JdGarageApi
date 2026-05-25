using Asp.Versioning;
using AutoMapper;
using JdGarageApi.Models;
using JdGarageApi.Models.DTOs;
using JdGarageApi.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;

namespace JdGarageApi.Controllers
{
    [Route("api/v{version:apiVersion}/cars")]
    public class CarsController : VehiclesController<Car, CarDto, CreateCarDto, UpdateCarDto>
    {
        public CarsController(
            IVehicleRepository<Car> repository,
            IBrandRepository brandRepository,
            IMapper mapper,
            CloudinaryService cloudinaryService)
            : base(repository, brandRepository, mapper, cloudinaryService, "car", "carros",
                  file => cloudinaryService.UploadCarImageAsync(file))
        {
        }
    }
}
