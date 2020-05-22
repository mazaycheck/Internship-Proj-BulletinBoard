using AutoMapper;
using Baraholka.Data.Dtos;
using Baraholka.Services.Models;
using System.Linq;

namespace Baraholka.Data.Configurations
{
    public class ServiceMappingProfile : Profile
    {
        public ServiceMappingProfile()
        {
            CreateMap<TownDto, TownModel>();
            CreateMap<TownCreateModel, TownDto>();
            CreateMap<TownUpdateModel, TownDto>();
            CreateMap<PageDataContainer<TownDto>, PageDataContainer<TownModel>>();

            CreateMap<UserDto, UserPublicWebModel>()
                .ForMember(dest => dest.TownName, sourse => sourse.MapFrom(src => src.Town.Title))
                .ForMember(dest => dest.UserId, sourse => sourse.MapFrom(src => src.Id));

            CreateMap<UserDto, UserAdminModel>()
                .ForMember(dest => dest.UserId, sourse => sourse.MapFrom(src => src.Id))
                .ForMember(dest => dest.TownName, sourse => sourse.MapFrom(src => src.Town.Title))
                .ForMember(dest => dest.Roles, sourse => sourse
                    .MapFrom(src => src.UserRoles.Select(x => x.Role.Name).ToList()));

            CreateMap<PageDataContainer<UserDto>, PageDataContainer<UserAdminModel>>();
        }
    }
}