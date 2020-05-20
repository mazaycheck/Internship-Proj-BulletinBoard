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

            CreateMap<BrandCategory, BrandCategoryModel>()
                .ForMember(dest => dest.BrandTitle, sourse => sourse.MapFrom(src => src.Brand.Title))
                .ForMember(dest => dest.CategoryTitle, sourse => sourse.MapFrom(src => src.Category.Title));

            CreateMap<Brand, BrandDto>().ReverseMap();
            CreateMap<PageDataContainer<Brand>, PageDataContainer<BrandDto>>();

            CreateMap<Category, CategoryBasicDto>().ReverseMap();

            CreateMap<MessageForCreateDto, Message>();
            CreateMap<Message, MessageForDetailDto>()
                .ForMember(dest => dest.SenderName, sourse => sourse.MapFrom(src => src.Sender.UserName))
                .ForMember(dest => dest.RecieverName, sourse => sourse.MapFrom(src => src.Reciever.UserName));

            CreateMap<User, UserForPublicDetail>()
                .ForMember(dest => dest.TownName, sourse => sourse.MapFrom(src => src.Town.Title))
                .ForMember(dest => dest.UserId, sourse => sourse.MapFrom(src => src.Id));

            CreateMap<UserRegisterDto, User>();

            CreateMap<User, UserForModeratorView>()
                .ForMember(dest => dest.UserId, sourse => sourse.MapFrom(src => src.Id))
                .ForMember(dest => dest.TownName, sourse => sourse.MapFrom(src => src.Town.Title))
                .ForMember(dest => dest.Roles, sourse => sourse
                    .MapFrom(src => src.UserRoles.Select(x => x.Role.Name).ToList()));

            CreateMap<User, UserServiceDto>().ReverseMap();

            CreateMap<PageDataContainer<User>, PageDataContainer<UserForModeratorView>>();

            CreateMap<Town, TownForPublicViewDto>();
            CreateMap<Town, TownServiceDto>().ReverseMap();
            CreateMap<TownForCreateDto, Town>();
            CreateMap<TownForUpdateDto, Town>();
            CreateMap<PageDataContainer<Town>, PageDataContainer<TownServiceDto>>();
        }
    }
}