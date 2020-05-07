using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration.UserSecrets;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using WebApplication2.Data.Dtos;
using WebApplication2.Data.Repositories;
using WebApplication2.Helpers;
using WebApplication2.Models;

namespace WebApplication2.Services
{
    public class AnnoucementService : IAnnoucementService
    {
        private readonly IAnnoucementRepository _repo;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _webHost;

        public AnnoucementService(IAnnoucementRepository repo, IMapper mapper, IWebHostEnvironment webHost)
        {
            _repo = repo;
            _mapper = mapper;
            _webHost = webHost;
        }

        public async Task<AnnoucementForViewDto> CreateNewAnnoucement(AnnoucementForCreateDto annoucementDto, int userId)
        {
            Annoucement annoucement = _mapper.Map<Annoucement>(annoucementDto);
            annoucement.UserId = userId;

            await _repo.Create(annoucement);
            await _repo.Save();
            
            if (annoucementDto.Photo != null)
            {
                string annoucementFolderForImageUpload = GetFolderNameForAnnoucementPhotos(annoucement.AnnoucementId);
                List<string> generatedImageNames = UploadImages(annoucementDto.Photo, annoucementFolderForImageUpload);
                AddPhotosToAnnoucement(annoucement, generatedImageNames);
                await _repo.Save();
            }

            return _mapper.Map<AnnoucementForViewDto>(annoucement);
        }

      

        private void AddPhotosToAnnoucement(Annoucement annoucement, List<string> listOfImgUrls)
        {
            annoucement.Photos = new List<Photo>();
            foreach (var photoPath in listOfImgUrls)
            {
                annoucement.Photos.Add(new Photo() { PhotoUrl = photoPath });
            }
        }

        private List<string> UploadImages(List<IFormFile> formImages, string folderName)
        {            
            var images = ImageFileProcessor.ConvertListIFormFileToListImage(formImages);
            List<string> listOfImgUrls = ImageFileProcessor.UploadFilesOnServerAndGetListOfFileNames(images, folderName);
            return listOfImgUrls;            
        }

        public async Task<AnnoucementForViewDto> GetAnnoucementById(int id)
        {
            var annoucement = await _repo.GetById(id);
            if (annoucement != null)
            {
                var annoucementDto = _mapper.Map<AnnoucementForViewDto>(annoucement);
            }
            return null;
        }

        public async Task<Paged<AnnoucementForViewDto>> GetAnnoucements(AnnoucementFilter filterOptions,
            PaginateParams paginateParams,OrderParams orderParams)
        {
            var annoucements = _repo.GetAll();
            IQueryable<AnnoucementForViewDto> annoucementsDto = annoucements.ProjectTo<AnnoucementForViewDto>(_mapper.ConfigurationProvider);
            var filteredAnnoucements = annoucementsDto.ApplySeachQuery(filterOptions);
            if (filteredAnnoucements.Count() > 0)
            {
                var orderedAnnoucements = filteredAnnoucements.OrderAnnoucements(orderParams);
                var pageData = await Paged<AnnoucementForViewDto>.Paginate(orderedAnnoucements, paginateParams);
                return pageData;
            }
            else
            {
                return null;
            }
        }

    

        public async Task<bool> DeleteAnnoucementById(int id)
        {
            var annoucement = await _repo.GetById(id);
            if (annoucement == null)
            {
                return false;
            }
            await _repo.Delete(annoucement);
            await _repo.Save();
            try
            {
                DeleteFolderWithAnnoucementPhotos(annoucement.AnnoucementId);
            }
            catch (InvalidCredentialException e)
            {

                throw;
            }
            
            return true;
        }

        private void DeleteFolderWithAnnoucementPhotos(int annoucementId)
        {
            string annoucementIdImageFolder = GetFolderNameForAnnoucementPhotos(annoucementId);
            ImageFileProcessor.DeleteFolderWithAnnoucementPhotos(annoucementIdImageFolder);
        }

        private string GetFolderNameForAnnoucementPhotos(int annoucementId)
        {
            return Path.Combine(_webHost.WebRootPath, "images", $"{annoucementId}");
        }


        public async Task<AnnoucementForViewDto> UpdateAnnoucement(AnnoucementForUpdateDto annoucementDto, int userId)
        {
            Annoucement annoucement = _mapper.Map<Annoucement>(annoucementDto);
            annoucement.UserId = userId;

            await _repo.Update(annoucement);
            await _repo.Save();

            if (annoucementDto.Photo != null)
            {
                string annoucementImageFolder = GetFolderNameForAnnoucementPhotos(annoucement.AnnoucementId);
                ImageFileProcessor.DeleteFolderWithAnnoucementPhotos(annoucementImageFolder);
                //var annoucementWithPhotos = await _repo.GetAll().Include(p => p.Photos).Where(x => x.AnnoucementId == annoucement.AnnoucementId).FirstOrDefaultAsync();
                //annoucementWithPhotos.Photos = new List<Photo>();
                List<string> generatedImageNames = UploadImages(annoucementDto.Photo, annoucementImageFolder);
                AddPhotosToAnnoucement(annoucement, generatedImageNames);
                await _repo.Save();
            }

            return _mapper.Map<AnnoucementForViewDto>(annoucement);

        }
    }
}
