using AutoMapper;
using MicroserviceTemplate.Application.Features.Vehicle;
using MicroserviceTemplate.Domain.Entities;

namespace MicroserviceTemplate.Application.ProfileMappers
{
    public class VehicleProfile : Profile
    {
        public VehicleProfile()
        {
            CreateMap<Vehicle, VehicleViewModel>()
                .ForMember(dest => dest.Vin, opt => opt.MapFrom(src => src.Vin))
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.UId));
        }
    }
}
