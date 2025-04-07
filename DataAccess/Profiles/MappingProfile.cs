using AutoMapper;
using DataAccess.Models;
using DataAccess.DTOs;
using DataAccess.Dtos;

namespace DataAccess.Profiles
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Brand, BrandDTO>().ReverseMap();

            CreateMap<CarCategory, CarCategoryDTO>().ReverseMap();

            CreateMap<CarModel, ModelDto>()
                .ForMember(dest => dest.BrandId, opt => opt.MapFrom(src => src.BrandId))
                .ReverseMap();

            CreateMap<Car, CarDTO>()
                
                
                
                .ReverseMap();


            CreateMap<Rental, RentalDTO>()
                //.ForMember(dest => dest.Car, opt => opt.MapFrom(src => src.Car.CarModel.Name))
                //.ForMember(dest => dest.CarLicensePlate, opt => opt.MapFrom(src => src.Car.LicensePlate))
                //.ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()))
                .ReverseMap();

            CreateMap<LoginDto, AppUser>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.Username));
        }
    }
}
