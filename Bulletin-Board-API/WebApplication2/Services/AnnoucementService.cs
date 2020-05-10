﻿using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Authentication;
using System.Security.Claims;
using System.Threading.Tasks;
using WebApplication2.Data.Dtos;
using WebApplication2.Data.Repositories;
using WebApplication2.Helpers;
using WebApplication2.Models;

namespace WebApplication2.Services
{
    public class AnnoucementService : IAnnoucementService
    {
        private readonly IAnnoucementRepository _annoucementRepo;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _webHost;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IGenericRepository<BrandCategory> _brandCategoryRepo;

        public AnnoucementService(IAnnoucementRepository annoucementRepo, IMapper mapper, IWebHostEnvironment webHost, IHttpContextAccessor contextAccessor, IGenericRepository<BrandCategory> brandCategoryRepo)
        {
            _annoucementRepo = annoucementRepo;
            _annoucementRepo = annoucementRepo;
            _mapper = mapper;
            _webHost = webHost;
            _contextAccessor = contextAccessor;
            _brandCategoryRepo = brandCategoryRepo;
        }

        public async Task<AnnoucementViewDto> CreateAnnoucement(AnnoucementCreateDto annoucementDto)
        {
            Annoucement annoucement = _mapper.Map<Annoucement>(annoucementDto);
            annoucement.UserId = GetUserIdFromClaims();
            if (!await _brandCategoryRepo.Exists(annoucementDto.BrandCategoryId))
            {
                throw new ArgumentException("No such category / brand combination");
            }
            annoucement.CreateDate = DateTime.Now;
            annoucement.ExpirationDate = DateTime.Now.AddDays(30);
            annoucement.IsActive = true;
            await _annoucementRepo.Create(annoucement);
            List<IFormFile> images = annoucementDto.Photo;
            if (images != null)
            {
                try
                {
                    await SaveAnnoucementImages(annoucement, images);
                }
                catch
                {
                    throw new Exception("Could not save images");
                }
            }
            return _mapper.Map<AnnoucementViewDto>(annoucement);
        }

        public async Task<AnnoucementViewDto> UpdateAnnoucement(AnnoucementUpdateDto annoucementDto)
        {
            var propertiesToInclude = new List<Expression<Func<Annoucement, object>>>
            {
                annoucement => annoucement.User
            };

            var collectionsToInclude = new List<Expression<Func<Annoucement, IEnumerable<object>>>>
            {
                annoucement => annoucement.Photos
            };
            Annoucement annoucementFromDb = await _annoucementRepo.GetById(annoucementDto.AnnoucementId, propertiesToInclude, collectionsToInclude);

            if (annoucementFromDb == null)
            {
                throw new NullReferenceException($"No annoucement with such id: {annoucementDto.AnnoucementId}");
            }
            if (!await _brandCategoryRepo.Exists(annoucementDto.BrandCategoryId))
            {
                throw new ArgumentException("No such category / brand combination");
            }

            //annoucementFromDb.CreateDate = annoucementFromDb.CreateDate;
            //annoucementFromDb.ExpirationDate = annoucementFromDb.ExpirationDate;
            //annoucementFromDb.Photos = annoucementFromDb.Photos;

            if (UserIdFromClaimsEquals(annoucementFromDb.UserId))
            {
                _mapper.Map<AnnoucementUpdateDto, Annoucement>(annoucementDto, annoucementFromDb);
                await _annoucementRepo.Update(annoucementFromDb);
                List<IFormFile> images = annoucementDto.Photo;
                if (images != null)
                {
                    await SaveAnnoucementImages(annoucementFromDb, images);
                }
                return _mapper.Map<AnnoucementViewDto>(annoucementFromDb);
            }
            else
            {
                throw new UnauthorizedAccessException("You cannot edit other user's annoucement");
            }
        }

        public async Task<PageDataContainer<AnnoucementViewDto>> GetAnnoucements(AnnoucementFilter filterOptions,
                     PaginateParams paginateParams, OrderParams orderParams)
        {
            PageDataContainer<Annoucement> pagedData = await _annoucementRepo.GetPagedAnnoucements(filterOptions, paginateParams, orderParams);
            if (pagedData == null)
            {
                throw new NullReferenceException("No data");
            }

            PageDataContainer<AnnoucementViewDto> pagedViewData = _mapper.Map<PageDataContainer<AnnoucementViewDto>>(pagedData);

            return pagedViewData;

        }

        public async Task<AnnoucementViewDto> GetAnnoucementById(int id)
        {

            Annoucement annoucement = await _annoucementRepo.GetAnnoucementById(id);
            if (annoucement != null)
            {
                return _mapper.Map<AnnoucementViewDto>(annoucement);
            }
            return null;
        }

        public async Task<bool> DeleteAnnoucementById(int annoucementId)
        {
            Annoucement annoucement = await _annoucementRepo.GetById(annoucementId);

            if (annoucement == null)
            {
                throw new NullReferenceException($"No annoucement with such id: {annoucementId}");
            }

            if (UserHasRequiredRoles("Admin", "Moderator") || UserIdFromClaimsEquals(annoucementId))
            {
                await _annoucementRepo.Delete(annoucement);
                DeleteFolderWithAnnoucementPhotos(annoucement.AnnoucementId);
                return true;
            }
            else
            {
                throw new UnauthorizedAccessException("You dont have rights to delete other member's annoucement");
            }
        }

        private async Task SaveAnnoucementImages(Annoucement annoucement, List<IFormFile> images)
        {
            string annoucementFolderForImageUpload = FolderForImages(annoucement.AnnoucementId);
            List<string> generatedImageNames = UploadImages(images, annoucementFolderForImageUpload);

            AddPhotosToAnnoucement(annoucement, generatedImageNames);
            await _annoucementRepo.Save();
        }

        private void AddPhotosToAnnoucement(Annoucement annoucement, List<string> listOfImgUrls)
        {
            annoucement.Photos = new List<Photo>();
            foreach (string photoPath in listOfImgUrls)
            {
                annoucement.Photos.Add(new Photo() { PhotoUrl = photoPath });
            }
        }

        private List<string> UploadImages(List<IFormFile> formImages, string folderName)
        {
            List<Image> images = ImageFileProcessor.ConvertIFormFileToImage(formImages);
            ImageFileProcessor.DeleteFolder(folderName);
            List<string> listOfImgUrls = ImageFileProcessor.UploadFilesOnServer(images, folderName);
            return listOfImgUrls;
        }

        private void DeleteFolderWithAnnoucementPhotos(int annoucementId)
        {
            string annoucementIdImageFolder = FolderForImages(annoucementId);
            ImageFileProcessor.DeleteFolder(annoucementIdImageFolder);
        }

        private string FolderForImages(int annoucementId)
        {
            return Path.Combine(_webHost.WebRootPath, "images", $"{annoucementId}");
        }

        private bool UserIdFromClaimsEquals(int id)
        {
            ClaimsPrincipal user = _contextAccessor.HttpContext.User;
            if (user.HasClaim(x => x.Type == ClaimTypes.NameIdentifier))
            {
                return id == Int32.Parse(user.FindFirst(ClaimTypes.NameIdentifier).Value);
            }
            else
            {
                throw new InvalidCredentialException($"User has no NameIdentifier claims");
            }
        }

        private int GetUserIdFromClaims()
        {
            ClaimsPrincipal user = _contextAccessor.HttpContext.User;
            if (user.HasClaim(x => x.Type == ClaimTypes.NameIdentifier))
            {
                return Int32.Parse(user.FindFirst(ClaimTypes.NameIdentifier).Value);
            }
            else
            {
                throw new InvalidCredentialException($"User has no NameIdentifier claims");
            }
        }

        private bool UserHasRequiredRoles(params string[] roles)
        {
            ClaimsPrincipal user = _contextAccessor.HttpContext.User;
            if (user.HasClaim(x => x.Type == ClaimTypes.Role))
            {
                bool result = roles.Any(role => user.IsInRole(role));
                if (!result)
                {
                    throw new UnauthorizedAccessException();
                }
                else
                    return true;
            }
            else
            {
                throw new InvalidCredentialException($"User has no Role claims");
            }
        }
    }
}