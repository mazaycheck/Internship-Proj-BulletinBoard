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
            CreateMap<Annoucement, AnnoucementDto>();
            CreateMap<AnnoucementDto, Annoucement>();

            CreateMap<AnnoucementDto, AnnoucementModel>()
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

            CreateMap<PageDataContainer<Annoucement>, PageDataContainer<AnnoucementDto>>();
            CreateMap<PageDataContainer<AnnoucementDto>, PageDataContainer<AnnoucementModel>>();

            CreateMap<AnnoucementCreateModel, AnnoucementDto>();
            CreateMap<AnnoucementUpdateModel, AnnoucementDto>()
                .ForMember(dest => dest.CreateDate, sourse => sourse.Ignore())
                .ForMember(dest => dest.ExpirationDate, sourse => sourse.Ignore())
                .ForMember(dest => dest.Photos, sourse => sourse.Ignore())
                .ForMember(dest => dest.UserId, sourse => sourse.Ignore());

            CreateMap<Category, CategoryDto>().ReverseMap();
            CreateMap<CategoryDto, CategoryModel>()
                .ForMember(dest => dest.Brands, sourse => sourse
                    .MapFrom(src => src.BrandCategories.Select(x => x.Brand.Title).ToList()));

            CreateMap<CategoryCreateModel, CategoryDto>();
            CreateMap<CategoryUpdateModel, CategoryDto>();

            CreateMap<BrandDto, BrandModel>()
                .ForMember(dest => dest.Categories, sourse => sourse
                    .MapFrom(src => src.BrandCategories.Select(x => x.Category.Title).ToList()))
                .ReverseMap();

            CreateMap<PageDataContainer<BrandDto>, PageDataContainer<BrandModel>>();
            CreateMap<BrandCreateModel, BrandDto>();
            CreateMap<BrandUpdateModel, BrandDto>();

            CreateMap<Brand, BrandModel>()
                .ForMember(dest => dest.Categories, sourse => sourse
                    .MapFrom(src => src.BrandCategories.Select(x => x.Category.Title).ToList()))
                .ReverseMap();
        }
    }
}