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
            CreateMap<AnnoucementCreateDto, Annoucement>();
            CreateMap<AnnoucementUpdateDto, Annoucement>()
                .ForMember(dest => dest.AnnoucementId, sourse => sourse.Ignore())
                .ForMember(dest => dest.CreateDate, sourse => sourse.Ignore())
                .ForMember(dest => dest.ExpirationDate, sourse => sourse.Ignore())
                .ForMember(dest => dest.Photos, sourse => sourse.Ignore());

            CreateMap<BrandCategory, BrandCategoryForViewDto>()
                .ForMember(dest => dest.BrandTitle, sourse => sourse.MapFrom(src => src.Brand.Title))
                .ForMember(dest => dest.CategoryTitle, sourse => sourse.MapFrom(src => src.Category.Title));

            CreateMap<Brand, BrandForViewDto>()
                .ForMember(dest => dest.Categories, sourse => sourse
                    .MapFrom(src => src.BrandCategories.Select(x => x.Category.Title).ToList()));

            CreateMap<Category, CategoryForViewDto>()
                .ForMember(dest => dest.Brands, sourse => sourse
                    .MapFrom(src => src.BrandCategories.Select(x => x.Brand.Title).ToList()));

            CreateMap<CategoryForCreateDto, Category>();

            CreateMap<BrandForCreateDto, Brand>();

            CreateMap<Annoucement, AnnoucementViewDto>()
                .ForMember(dest => dest.Id, sourse => sourse
                    .MapFrom(src => src.AnnoucementId))
                .ForMember(dest => dest.Date, sourse => sourse
                    .MapFrom(src => src.CreateDate))
                .ForMember(dest => dest.UserId, sourse => sourse
                    .MapFrom(src => src.UserId))
                 .ForMember(dest => dest.Category, sourse => sourse
                    .MapFrom(src => src.BrandCategory.Category.Title))
                 .ForMember(dest => dest.Brand, sourse => sourse
                    .MapFrom(src => src.BrandCategory.Brand.Title))
                 .ForMember(dest => dest.Town, sourse => sourse
                    .MapFrom(src => src.User.Town.Title))
                 .ForMember(dest => dest.PhotoUrls, sourse => sourse
                    .MapFrom(src => src.Photos.Select(x => x.PhotoUrl).ToList()));

            CreateMap<Annoucement, AnnoucementCheckDto>()
                .ForMember(dest => dest.Id, sourse => sourse
                    .MapFrom(src => src.AnnoucementId)).ReverseMap();

            CreateMap<PageDataContainer<Annoucement>, PageDataContainer<AnnoucementViewDto>>();
            CreateMap<PageDataContainer<Brand>, PageDataContainer<BrandForViewDto>>();

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

            CreateMap<PageDataContainer<User>, PageDataContainer<UserForModeratorView>>();

            CreateMap<Town, TownForPublicViewDto>();
            CreateMap<Town, TownForAdminViewDto>();
            CreateMap<TownForCreateDto, Town>();
            CreateMap<TownForUpdateDto, Town>();
            CreateMap<PageDataContainer<Town>, PageDataContainer<TownForAdminViewDto>>();
        }
    }
}