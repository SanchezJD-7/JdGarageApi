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
    public class BikesController : VehiclesController<Bike, BikeDto, CreateBikeDto, UpdateBikeDto>
    {
        public BikesController(
            IVehicleRepository<Bike> repository,
            IBrandRepository brandRepository,
            IMapper mapper,
            CloudinaryService cloudinaryService)
            : base(repository, brandRepository, mapper, cloudinaryService, "bike", "motocicletas",
                  file => cloudinaryService.UploadBikeImageAsync(file))
        {
        }
    }
}
