using AutoMapper;
using Baraholka.Data.Dtos;
using Baraholka.Data.Repositories;
using Baraholka.Domain.Models;
using Baraholka.Services.Models;
using Baraholka.Utilities;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Baraholka.Services
{
    public class AnnoucementService : IAnnoucementService
    {
        private readonly IAnnoucementRepository _annoucementRepo;
        private readonly IMapper _mapper;
        private readonly IGenericRepository<BrandCategory> _brandCategoryRepo;
        private readonly IImageFileManager _imageFileManager;

        public AnnoucementService(
                IAnnoucementRepository annoucementRepo,
                IMapper mapper,
                IGenericRepository<BrandCategory> brandCategoryRepo,
                IImageFileManager imageFileManager
                )
        {
            _annoucementRepo = annoucementRepo;
            _mapper = mapper;
            _brandCategoryRepo = brandCategoryRepo;
            _imageFileManager = imageFileManager;
        }

        public async Task<AnnoucementModel> CreateAnnoucement(AnnoucementCreateModel annoucementDto, int userId)
        {
            AnnoucementDto annoucementToCreate = _mapper.Map<AnnoucementDto>(annoucementDto);
            annoucementToCreate.UserId = userId;
            AnnoucementDto createdAnnoucement = await _annoucementRepo.CreateAnnoucement(annoucementToCreate);

            List<IFormFile> images = annoucementDto.Photo;

            if (images != null)
            {
                await SaveAnnoucementImages(createdAnnoucement.AnnoucementId, images);
            }

            return _mapper.Map<AnnoucementModel>(createdAnnoucement);
        }

        public async Task<AnnoucementModel> UpdateAnnoucement(AnnoucementUpdateModel annoucementDto, int userId)
        {
            AnnoucementDto annoucementSourse = _mapper.Map<AnnoucementDto>(annoucementDto);
            annoucementSourse.UserId = userId;
            AnnoucementDto updatedAnnoucement = await _annoucementRepo.UpdateAnnoucement(annoucementSourse);

            List<IFormFile> images = annoucementDto.Photo;

            if (images != null)
            {
                _imageFileManager.DeleteOldImages(updatedAnnoucement.AnnoucementId);

                await SaveAnnoucementImages(updatedAnnoucement.AnnoucementId, images);
            }

            return _mapper.Map<AnnoucementModel>(updatedAnnoucement);
        }

        public async Task<PageDataContainer<AnnoucementModel>> GetAnnoucements(AnnoucementFilterArguments filterOptions,
             PageArguments paginateParams, SortingArguments orderParams)
        {
            PageDataContainer<AnnoucementDto> pagedAnnoucements = await _annoucementRepo.GetPagedAnnoucements(filterOptions, paginateParams, orderParams);

            if (pagedAnnoucements.PageData.Count == 0)
            {
                return null;
            }
            PageDataContainer<AnnoucementModel> pagedViewData = _mapper.Map<PageDataContainer<AnnoucementModel>>(pagedAnnoucements);

            return pagedViewData;
        }

        public async Task<AnnoucementModel> GetAnnoucement(int id)
        {
            AnnoucementDto annoucement = await _annoucementRepo.GetSingleAnnoucementForView(id);
            if (annoucement != null)
            {
                var annoucementForView = _mapper.Map<AnnoucementModel>(annoucement);
                return annoucementForView;
            }
            return null;
        }

        public async Task DeleteAnnoucement(int id)
        {
            await _annoucementRepo.Delete(new Annoucement() { AnnoucementId = id });

            _imageFileManager.DeleteOldImages(id);
        }

        public async Task<bool> BrandCategoryExists(int brandCategoryId)
        {
            return await _brandCategoryRepo.Exists(b => b.BrandCategoryId == brandCategoryId);
        }

        private async Task SaveAnnoucementImages(int annoucementId, List<IFormFile> images)
        {
            List<string> fileGuidNames = _imageFileManager.UploadImageFiles(images, folderName: $"{annoucementId}");

            await _annoucementRepo.SaveImageFileNames(annoucementId, fileGuidNames);
        }
    }
}