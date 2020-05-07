using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using WebApplication2.Data.Dtos;
using WebApplication2.Models;

namespace WebApplication2.Helpers
{
    public class AutoMapperConfig : Profile
    {
        public AutoMapperConfig()
        {
            CreateMap<AnnoucementForCreateDto, Annoucement>();
            CreateMap<AnnoucementForUpdateDto, Annoucement>();

            CreateMap<BrandCategoryDto, BrandCategory>();

            CreateMap<Annoucement, AnnoucementForViewDto>()
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
                    .MapFrom(src => src.UserRoles.Select(x=> x.Role.Name).ToList()));
                
                
        }
    }
}
