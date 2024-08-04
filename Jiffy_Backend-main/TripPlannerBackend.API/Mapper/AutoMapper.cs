using AutoMapper;
using JiffyBackend.API.Dto;
using JiffyBackend.DAL.Entity;

namespace JiffyBackend.API.Mapper
{
    public class AutoMapper : Profile
    {
        public AutoMapper()
        {
            CreateMap<ServiceType, ServiceTypeDto>();
            CreateMap<CreateServiceTypeDto, ServiceType>();
            CreateMap<UpdateServiceTypeDto, ServiceType>();

            CreateMap<Service, ServiceDto>();
            CreateMap<CreateServiceDto, Service>();
            CreateMap<UpdateServiceDto, Service>();

            CreateMap<User, UserDto>();
            CreateMap<CreateUserDto, User>();
            CreateMap<UpdateUserDto, User>();

            CreateMap<Booking, BookingDto>()
                .ForMember(dest => dest.Booker, opt => opt.MapFrom(src => src.Booker))
                .ForMember(dest => dest.Service, opt => opt.MapFrom(src => src.Service));

            CreateMap<CreateBookingDto, Booking>();
        }
    }
}
