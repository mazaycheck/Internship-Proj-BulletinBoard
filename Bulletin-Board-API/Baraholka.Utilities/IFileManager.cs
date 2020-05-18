using Baraholka.Utilities;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Drawing;

namespace Baraholka.Services.Services
{
    public interface IFileManager
    {
        void DeleteOldImages(string rootFolder, int annoucementId);

        void SaveResizedImages(Image imageFromForm, string filePath, int size);

        List<string> UploadImageFilesOnServer(List<Image> annoucementPhotoFiles, string annoucementIdImageFolder, List<ImageFolder> folders);

        List<string> UploadImages(List<IFormFile> formImages, string rootFolder, string folderName, List<ImageFolder> imageFolders);
    }
}