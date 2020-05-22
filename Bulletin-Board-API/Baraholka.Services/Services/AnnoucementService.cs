using Baraholka.Data.Dtos;
using Baraholka.Data.Repositories;
using Baraholka.Domain.Models;
using Baraholka.Utilities;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Baraholka.Services
{
    public class AnnoucementService : IAnnoucementService
    {
        private readonly IAnnoucementRepository _annoucementRepo;
        private readonly IGenericRepository<BrandCategory> _brandCategoryRepo;
        private readonly IImageFileManager _imageFileManager;

        public AnnoucementService(
                IAnnoucementRepository annoucementRepo,
                IGenericRepository<BrandCategory> brandCategoryRepo,
                IImageFileManager imageFileManager
                )
        {
            _annoucementRepo = annoucementRepo;
            _brandCategoryRepo = brandCategoryRepo;
            _imageFileManager = imageFileManager;
        }

        public async Task<AnnoucementDto> CreateAnnoucement(AnnoucementDto annoucementDto, List<IFormFile> images)
        {

            bool exists = await BrandCategoryExists(annoucementDto.BrandCategoryId);
            if (!exists)
            {
                throw new Exception("BrandCategory Id does not exist");
            }
                
            AnnoucementDto createdAnnoucement = await _annoucementRepo.CreateAnnoucement(annoucementDto);

            if (images != null)
            {
                await SaveAnnoucementImages(createdAnnoucement.AnnoucementId, images);
            }

            return createdAnnoucement;
        }

        public async Task<AnnoucementDto> UpdateAnnoucement(AnnoucementDto annoucementDto, List<IFormFile> images)
        {

            bool exists = await BrandCategoryExists(annoucementDto.BrandCategoryId);

            if (!exists)
            {
                throw new Exception("BrandCategory Id does not exist");
            }

            AnnoucementDto updatedAnnoucement = await _annoucementRepo.UpdateAnnoucement(annoucementDto);

            if (images != null)
            {
                _imageFileManager.DeleteOldImages(updatedAnnoucement.AnnoucementId);

                await SaveAnnoucementImages(updatedAnnoucement.AnnoucementId, images);
            }

            return annoucementDto;
        }

        public async Task<PageDataContainer<AnnoucementDto>> GetAnnoucements(AnnoucementFilterArguments filterOptions,
             PageArguments paginateParams, SortingArguments orderParams)
        {
            PageDataContainer<AnnoucementDto> pagedAnnoucements = await _annoucementRepo.GetPagedAnnoucements(filterOptions, paginateParams, orderParams);

            if (pagedAnnoucements.PageData.Count == 0)
            {
                return null;
            }

            return pagedAnnoucements;
        }

        public async Task<AnnoucementDto> GetAnnoucement(int id)
        {
            AnnoucementDto annoucement = await _annoucementRepo.GetSingleAnnoucementForView(id);
            if (annoucement != null)
            {
                return annoucement;
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