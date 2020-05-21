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
            CreateMap<Annoucement, AnnoucementDto>();
            CreateMap<AnnoucementDto, Annoucement>();
            CreateMap<PageDataContainer<Annoucement>, PageDataContainer<AnnoucementDto>>();

            CreateMap<BrandCategory, BrandCategoryDto>().ReverseMap();

            CreateMap<Brand, BrandDto>().ReverseMap();
            CreateMap<PageDataContainer<Brand>, PageDataContainer<BrandDto>>();

            CreateMap<User, UserForPublicDetail>()
                .ForMember(dest => dest.TownName, sourse => sourse.MapFrom(src => src.Town.Title))
                .ForMember(dest => dest.UserId, sourse => sourse.MapFrom(src => src.Id));

            CreateMap<UserRegisterDto, User>();

            CreateMap<User, UserAdminModel>()
                .ForMember(dest => dest.UserId, sourse => sourse.MapFrom(src => src.Id))
                .ForMember(dest => dest.TownName, sourse => sourse.MapFrom(src => src.Town.Title))
                .ForMember(dest => dest.Roles, sourse => sourse
                    .MapFrom(src => src.UserRoles.Select(x => x.Role.Name).ToList()));

            CreateMap<User, UserServiceDto>().ReverseMap();

            CreateMap<PageDataContainer<User>, PageDataContainer<UserAdminModel>>();

            CreateMap<Town, TownDto>().ReverseMap();

            CreateMap<PageDataContainer<Town>, PageDataContainer<TownDto>>();

            CreateMap<Message, MessageDto>();
            CreateMap<MessageDto, Message>()
                .ForMember(dest => dest.Sender, src => src.Ignore())
                .ForMember(dest => dest.Reciever, src => src.Ignore());

        }
    }
}