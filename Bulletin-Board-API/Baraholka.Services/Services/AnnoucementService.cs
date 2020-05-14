using AutoMapper;
using Baraholka.Data.Dtos;
using Baraholka.Data.Repositories;
using Baraholka.Domain.Models;
using Baraholka.Utilities;
using Baraholka.Web.Helpers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Baraholka.Services
{
    public class AnnoucementService : IAnnoucementService
    {
        private readonly IAnnoucementRepository _annoucementRepo;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _webHost;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IGenericRepository<BrandCategory> _brandCategoryRepo;
        private readonly IImageFileProcessor _imageFileProcessor;
        private readonly IPageService<Annoucement> _pageService;

        public AnnoucementService(
            IAnnoucementRepository annoucementRepo,
            IMapper mapper, IWebHostEnvironment webHost,
            IHttpContextAccessor contextAccessor,
            IGenericRepository<BrandCategory> brandCategoryRepo,
            IImageFileProcessor imageFileProcessor,
            IPageService<Annoucement> pageService
            )
        {
            _annoucementRepo = annoucementRepo;
            _annoucementRepo = annoucementRepo;
            _mapper = mapper;
            _webHost = webHost;
            _contextAccessor = contextAccessor;
            _brandCategoryRepo = brandCategoryRepo;
            _imageFileProcessor = imageFileProcessor;
            _pageService = pageService;
        }

        public async Task<AnnoucementViewDto> CreateAnnoucement(AnnoucementCreateDto annoucementDto, int userId)
        {
            Annoucement annoucement = _mapper.Map<Annoucement>(annoucementDto);
            SetDefaultValues(userId, annoucement);
            await _annoucementRepo.Create(annoucement);

            List<IFormFile> images = annoucementDto.Photo;
            if (images != null)
            {
                await SaveAnnoucementImages(annoucement, images);
            }

            return _mapper.Map<AnnoucementViewDto>(annoucement);
        }

        private void SetDefaultValues(int userId, Annoucement annoucement)
        {
            annoucement.UserId = userId;
            annoucement.CreateDate = DateTime.Now;
            annoucement.ExpirationDate = DateTime.Now.AddDays(30);
            annoucement.IsActive = true;
        }

        public async Task<AnnoucementViewDto> UpdateAnnoucement(AnnoucementUpdateDto annoucementDto)
        {
            var collectionsToInclude = new List<Expression<Func<Annoucement, IEnumerable<object>>>>
            {
                annoucement => annoucement.Photos
            };

            var testIncludes = new string[] { $"{nameof(Annoucement.Photos)}" };

            Annoucement annoucementFromDb = await _annoucementRepo.FindById(annoucementDto.AnnoucementId, testIncludes);

            _mapper.Map<AnnoucementUpdateDto, Annoucement>(annoucementDto, annoucementFromDb);
            await _annoucementRepo.Update(annoucementFromDb);
            List<IFormFile> images = annoucementDto.Photo;
            if (images != null)
            {
                await SaveAnnoucementImages(annoucementFromDb, images);
            }
            return _mapper.Map<AnnoucementViewDto>(annoucementFromDb);
        }

        public async Task<PageDataContainer<AnnoucementViewDto>> GetAnnoucements(AnnoucementFilterArguments filterOptions,
                     PageArguments paginateParams, SortingArguments orderParams)
        {
            IOrderedQueryable<Annoucement> annoucements = _annoucementRepo.GetAnnoucementsForPaging(filterOptions, paginateParams, orderParams);

            PageDataContainer<Annoucement> pagedAnnoucements = await _pageService.Paginate(annoucements, paginateParams);

            if (pagedAnnoucements.PageData.Count == 0)
            {
                return null;
            }
            PageDataContainer<AnnoucementViewDto> pagedViewData = _mapper.Map<PageDataContainer<AnnoucementViewDto>>(pagedAnnoucements);

            return pagedViewData;
        }

        public async Task<AnnoucementViewDto> GetAnnoucementForViewById(int id)
        {
            Annoucement annoucement = await _annoucementRepo.GetSingleAnnoucementForViewById(id);
            if (annoucement != null)
            {
                var annoucementForView = _mapper.Map<AnnoucementViewDto>(annoucement);
                return annoucementForView;
            }
            return null;
        }

        public async Task<AnnoucementMinimalDto> GetAnnoucementForValidateById(int id)
        {
            Annoucement annoucement = await _annoucementRepo.GetSingle(x => x.AnnoucementId == id);
            if (annoucement != null)
            {
                return _mapper.Map<AnnoucementMinimalDto>(annoucement);
            }
            return null;
        }

        public async Task DeleteAnnoucementById(int id)
        {
            await _annoucementRepo.Delete(new Annoucement() { AnnoucementId = id });
            DeleteFolderWithAnnoucementPhotos(id);
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
            List<Image> images = _imageFileProcessor.ConvertIFormFileToImage(formImages);
            _imageFileProcessor.DeleteFolder(folderName);
            List<string> listOfImgUrls = _imageFileProcessor.UploadFilesOnServer(images, folderName);
            return listOfImgUrls;
        }

        private void DeleteFolderWithAnnoucementPhotos(int annoucementId)
        {
            string annoucementIdImageFolder = FolderForImages(annoucementId);
            _imageFileProcessor.DeleteFolder(annoucementIdImageFolder);
        }

        private string FolderForImages(int annoucementId)
        {
            return Path.Combine(_webHost.WebRootPath, "images", $"{annoucementId}");
        }

        public async Task<bool> BrandCategoryExists(int brandCategoryId)
        {
            return await _brandCategoryRepo.Exists(brandCategoryId);
        }
    }
}