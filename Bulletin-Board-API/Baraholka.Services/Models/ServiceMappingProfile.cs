using AutoMapper;
using Baraholka.Data.Dtos;
using Baraholka.Services.Models;

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
        }
    }
}