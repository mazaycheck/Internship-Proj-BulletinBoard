using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Drawing;

namespace Baraholka.Utilities
{
    public interface IImageFileManager
    {
        void SaveResizedImages(Image imageFromForm, string filePath, int size);

        List<string> UploadImageFilesOnServer(List<Image> annoucementPhotoFiles, string annoucementIdImageFolder, List<ImageFolderConfig> folders);

        void DeleteOldImages(int annoucementId);

        List<string> UploadImageFiles(List<IFormFile> formImages, string folderName);
    }
}