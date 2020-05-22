using AutoMapper;
using Baraholka.Data.Dtos;
using Baraholka.Domain.Models;
using Baraholka.Services.Models;
using System.Linq;

namespace Baraholka.Data.Configurations
{
    public class ServiceMappingProfile : Profile
    {
        public ServiceMappingProfile()
        {
            CreateMap<Category, CategoryDto>().ReverseMap();
            CreateMap<CategoryDto, CategoryModel>()
                .ForMember(dest => dest.Brands, sourse => sourse
                    .MapFrom(src => src.BrandCategories.Select(x => x.Brand.Title).ToList()));

            CreateMap<CategoryCreateModel, CategoryDto>();
            CreateMap<CategoryUpdateModel, CategoryDto>();

            CreateMap<BrandCategoryDto, BrandCategoryModel>()
                .ForMember(dest => dest.BrandTitle, sourse => sourse.MapFrom(src => src.Brand.Title))
                .ForMember(dest => dest.CategoryTitle, sourse => sourse.MapFrom(src => src.Category.Title));

            CreateMap<MessageCreateModel, MessageDto>();
            CreateMap<MessageDto, MessageModel>()
                .ForMember(dest => dest.SenderName, sourse => sourse.MapFrom(src => src.Sender.UserName))
                .ForMember(dest => dest.RecieverName, sourse => sourse.MapFrom(src => src.Reciever.UserName));

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