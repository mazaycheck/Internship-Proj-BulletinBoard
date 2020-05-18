using AutoMapper;
using Baraholka.Data.Dtos;
using Baraholka.Data.Repositories;
using Baraholka.Domain.Models;
using Baraholka.Services.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Baraholka.Services
{
    public class AnnoucementService : IAnnoucementService
    {
        private readonly IAnnoucementRepository _annoucementRepo;
        private readonly IMapper _mapper;
        private readonly IGenericRepository<BrandCategory> _brandCategoryRepo;
        private readonly IFileManager _imageFileManager;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly string _rootPath;

        public AnnoucementService(
            IAnnoucementRepository annoucementRepo,
            IMapper mapper,
            IGenericRepository<BrandCategory> brandCategoryRepo,
            IFileManager imageFileManager,
            IWebHostEnvironment webHostEnvironment
            )
        {
            _annoucementRepo = annoucementRepo;
            _mapper = mapper;
            _brandCategoryRepo = brandCategoryRepo;
            _imageFileManager = imageFileManager;
            _webHostEnvironment = webHostEnvironment;
            _rootPath = webHostEnvironment.WebRootPath;
        }

        public async Task<AnnoucementViewDto> CreateAnnoucement(AnnoucementCreateDto annoucementDto, int userId)
        {
            Annoucement annoucement = _mapper.Map<Annoucement>(annoucementDto);

            SetDefaultValuesForCreate(userId, annoucement);

            await _annoucementRepo.Create(annoucement);

            List<IFormFile> images = annoucementDto.Photo;

            if (images != null)
            {
                await SaveAnnoucementImages(annoucement, images);
            }

            return _mapper.Map<AnnoucementViewDto>(annoucement);
        }

        private void SetDefaultValuesForCreate(int userId, Annoucement annoucement)
        {
            annoucement.UserId = userId;
            annoucement.CreateDate = DateTime.Now;
            annoucement.ExpirationDate = DateTime.Now.AddDays(30);
            annoucement.IsActive = true;
        }

        public async Task<AnnoucementViewDto> UpdateAnnoucement(AnnoucementUpdateDto annoucementDto)
        {
            var includes = new string[] { $"{nameof(Annoucement.Photos)}" };

            Annoucement annoucementFromDb = await _annoucementRepo.FindById(annoucementDto.AnnoucementId, includes);

            _mapper.Map<AnnoucementUpdateDto, Annoucement>(annoucementDto, annoucementFromDb);

            await _annoucementRepo.Update(annoucementFromDb);

            List<IFormFile> images = annoucementDto.Photo;

            if (images != null)
            {
                _imageFileManager.DeleteOldImages(_rootPath, annoucementFromDb.AnnoucementId);

                await SaveAnnoucementImages(annoucementFromDb, images);
            }

            return _mapper.Map<AnnoucementViewDto>(annoucementFromDb);
        }

        public async Task<PageDataContainer<AnnoucementViewDto>> GetAnnoucements(AnnoucementFilterArguments filterOptions,
                     PageArguments paginateParams, SortingArguments orderParams)
        {
            PageDataContainer<Annoucement> pagedAnnoucements = await _annoucementRepo.GetPagedAnnoucements(filterOptions, paginateParams, orderParams);

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

        public async Task<AnnoucementUserInfoDto> GetAnnoucementUserInfo(int id)
        {
            Annoucement annoucement = await _annoucementRepo.GetSingle(x => x.AnnoucementId == id);
            if (annoucement != null)
            {
                return _mapper.Map<AnnoucementUserInfoDto>(annoucement);
            }
            return null;
        }

        public async Task DeleteAnnoucementById(int id)
        {
            await _annoucementRepo.Delete(new Annoucement() { AnnoucementId = id });

            _imageFileManager.DeleteOldImages(_webHostEnvironment.WebRootPath, id);
        }

        public async Task<bool> BrandCategoryExists(int brandCategoryId)
        {
            return await _brandCategoryRepo.Exists(brandCategoryId);
        }

        private async Task SaveAnnoucementImages(Annoucement annoucement, List<IFormFile> images)
        {
            List<string> fileGuidNames = _imageFileManager.UploadImages(images, _rootPath, folderName: $"{annoucement.AnnoucementId}");

            await _annoucementRepo.BindImages(annoucement, fileGuidNames);
        }
    }
}