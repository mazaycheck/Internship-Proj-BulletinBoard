using AutoMapper;
using Baraholka.Data.Dtos;
using Baraholka.Domain.Models;
using System.Linq;

namespace Baraholka.Data.Configurations
{
    public class AutoMapperConfig : Profile
    {
        public AutoMapperConfig()
        {
            CreateMap<Annoucement, AnnoucementDto>().ReverseMap();
            CreateMap<PageDataContainer<Annoucement>, PageDataContainer<AnnoucementDto>>();

            CreateMap<BrandCategory, BrandCategoryDto>().ReverseMap();

            CreateMap<Brand, BrandDto>().ReverseMap();
            CreateMap<PageDataContainer<Brand>, PageDataContainer<BrandDto>>();

            CreateMap<User, UserDto>()
                .ForMember(dest => dest.Roles, sourse => sourse
                    .MapFrom(src => src.UserRoles.Select(r => r.Role.Name).ToList()));

            CreateMap<UserDto, User>()
                .ForMember(dest => dest.Town, src => src.Ignore())
                .ForMember(dest => dest.UserRoles, src => src.Ignore());

            CreateMap<PageDataContainer<User>, PageDataContainer<UserDto>>();

            CreateMap<Town, TownDto>().ReverseMap();

            CreateMap<PageDataContainer<Town>, PageDataContainer<TownDto>>();

            CreateMap<Message, MessageDto>();
            CreateMap<MessageDto, Message>()
                .ForMember(dest => dest.Sender, src => src.Ignore())
                .ForMember(dest => dest.Reciever, src => src.Ignore());
        }
    }
}