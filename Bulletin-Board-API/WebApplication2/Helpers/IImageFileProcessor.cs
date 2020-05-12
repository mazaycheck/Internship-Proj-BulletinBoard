using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Drawing;

namespace WebApplication2.Helpers
{
    public interface IImageFileProcessor
    {
        List<Image> ConvertIFormFileToImage(List<IFormFile> formFiles);

        void DeleteFolder(string imagesPath);

        Image ResizeImage(Image imgToResize, Size size);

        void SaveImageResized(Image imageFromForm, string filePath, int width, int height);

        List<string> UploadFilesOnServer(List<Image> annoucementPhotoFiles, string annoucementIdImageFolder);
    }
}