using Baraholka.Domain.Models;
using Baraholka.Utilities;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;

namespace Baraholka.Services.Services
{
    public class ImageFileManagerService : IImageFileManagerService
    {
        private readonly IImageFileProcessor _imageFileProcessor;
        private readonly IRootFolderPath _rootFolder;

        public ImageFileManagerService(IImageFileProcessor imageFileProcessor, IRootFolderPath rootFolder)
        {
            _imageFileProcessor = imageFileProcessor;
            _rootFolder = rootFolder;
        }

        private void AddPhotosToAnnoucement(Annoucement annoucement, List<string> listOfImgUrls)
        {
            annoucement.Photos = new List<Photo>();
            foreach (string photoPath in listOfImgUrls)
            {
                annoucement.Photos.Add(new Photo() { PhotoUrl = photoPath });
            }
        }

        public List<string> UploadImages(List<IFormFile> formImages, string folderName)
        {
            List<Image> images = _imageFileProcessor.ConvertIFormFileToImage(formImages);
            var path = GetImagesFolderPath(folderName);
            List<string> listOfImgUrls = UploadImageFilesOnServer(images, path);
            return listOfImgUrls;
        }

        public void DeleteOldImages(int annoucementId)
        {
            string annoucementIdImageFolder = GetImagesFolderPath($"{annoucementId}");

            DeleteFolder(annoucementIdImageFolder);
        }

        private string GetImagesFolderPath(string id)
        {
            return Path.Combine(_rootFolder.FolderPath, "images", id);
        }


        public List<string> UploadImageFilesOnServer(List<Image> annoucementPhotoFiles, string annoucementIdImageFolder)
        {
            var listOfImgUrls = new List<string>();

            string smallImageFolder = Path.Combine(annoucementIdImageFolder, "small");
            string mediumImageFolder = Path.Combine(annoucementIdImageFolder, "medium");
            string largeImageFolder = Path.Combine(annoucementIdImageFolder, "large");

            CreateDirectories(annoucementIdImageFolder, smallImageFolder, mediumImageFolder, largeImageFolder);

            for (int i = 0; i < annoucementPhotoFiles.Count; i++)
            {
                Image imageFile = annoucementPhotoFiles[i];
                string guidFileName = Guid.NewGuid().ToString() + ".jpg";

                string largeImageFilePath = Path.Combine(largeImageFolder, guidFileName);
                string mediumImageFilePath = Path.Combine(mediumImageFolder, guidFileName);
                string smallImageFilePath = Path.Combine(smallImageFolder, guidFileName);

                SaveResizedImages(imageFile, largeImageFilePath, 1000, 1000);
                SaveResizedImages(imageFile, mediumImageFilePath, 500, 500);
                SaveResizedImages(imageFile, smallImageFilePath, 200, 200);

                listOfImgUrls.Add(guidFileName);
            }

            return listOfImgUrls;
        }

        public void SaveResizedImages(Image imageFromForm, string filePath, int width, int height)
        {
            Image imageResized;

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                imageResized = _imageFileProcessor.ResizeImage(imageFromForm, new Size(width, height));
                System.Drawing.Imaging.ImageFormat format = System.Drawing.Imaging.ImageFormat.Jpeg;
                imageResized.Save(fileStream, format);
            }
        }

        private void CreateDirectories(params string[] directories)
        {
            foreach (var dir in directories)
            {
                if (!Directory.Exists(dir))
                {
                    Directory.CreateDirectory(dir);
                }
            }
        }

        public void DeleteFolder(string imagesPath)
        {
            if (Directory.Exists(imagesPath))
            {
                try
                {
                    Directory.Delete(imagesPath, true);
                }
                catch (Exception e)
                {
                    throw new Exception($"Could not delete Folder with images on {imagesPath}");
                }
            }
        }

    }
}
