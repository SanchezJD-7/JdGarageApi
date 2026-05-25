using JdGarageApi.Models;
using JdGarageApi.Models.DTOs;
using AutoMapper;

namespace JdGarageApi.BikesMappers
{
    public class BikesMapper : Profile
    {
        public BikesMapper()
        {
            CreateMap<BikeCategory, BikeCategoryDto>().ReverseMap();
            CreateMap<BikeCategory, CreateBikeCategoryDto>().ReverseMap(); 
            CreateMap<Bike, BikeDto>().ReverseMap();
            CreateMap<Bike, CreateBikeDto>().ReverseMap();
            CreateMap<UpdateBikeDto, Bike>().ForAllMembers(opt =>opt.Condition((src, dest, srcMember) => srcMember != null));
            CreateMap<AppUser, UserDataDto>().ReverseMap();
            CreateMap<AppUser, UserDto>().ReverseMap();
            CreateMap<Brands, CreateBrandDto>().ReverseMap();
            CreateMap<Brands, UpdateBrandDto>().ReverseMap();
            CreateMap<Vehicle, VehicleDto>();
            CreateMap<Vehicle, CreateVehicleDto>().ReverseMap();
            CreateMap<UpdateVehicleDto, Vehicle>().ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));
            CreateMap<Car, CarDto>().ReverseMap();
            CreateMap<Car, CreateCarDto>().ReverseMap();
            CreateMap<UpdateCarDto, Car>().ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));
        }
    }
}
