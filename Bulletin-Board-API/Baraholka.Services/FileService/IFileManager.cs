using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Drawing;

namespace Baraholka.Services.Services
{
    public interface IFileManager
    {
        void DeleteOldImages(int annoucementId);

        void SaveResizedImages(Image imageFromForm, string filePath, int width, int height);

        List<string> UploadImages(List<IFormFile> formImages, string folderName);
    }
}