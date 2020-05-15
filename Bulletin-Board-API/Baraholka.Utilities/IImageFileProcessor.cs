using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Drawing;

namespace Baraholka.Utilities
{
    public interface IImageFileProcessor
    {
        List<Image> ConvertIFormFileToImage(List<IFormFile> formFiles);

        void DeleteFolder(string imagesPath);

        Image ResizeImage(Image imgToResize, Size size);

        void SaveResizedImages(Image imageFromForm, string filePath, int width, int height);

        List<string> UploadImageFilesOnServer(List<Image> annoucementPhotoFiles, string annoucementIdImageFolder);
    }
}