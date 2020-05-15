using Baraholka.Domain.Models;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Drawing;
using System.Threading.Tasks;

namespace Baraholka.Services.Services
{
    public interface IImageFileManagerService
    {
        void DeleteOldImages(int annoucementId);

        void SaveResizedImages(Image imageFromForm, string filePath, int width, int height);

        List<string> UploadImages(List<IFormFile> formImages, string folderName);
    }
}