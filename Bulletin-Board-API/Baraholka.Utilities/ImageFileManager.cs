using Baraholka.Utilities;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;

namespace Baraholka.Services.Services
{
    public class ImageFileManager : IImageFileManager
    {
        private readonly IImageFileProcessor _imageFileProcessor;

        public ImageFileManager(IImageFileProcessor imageFileProcessor)
        {
            _imageFileProcessor = imageFileProcessor;
        }

        public List<string> UploadImageFiles(List<IFormFile> formImages, string rootFolder, string folderName, List<ImageFolderConfig> imageFolders)
        {
            List<Image> images = _imageFileProcessor.ConvertIFormFileToImage(formImages);
            var path = GetImagesFolderPath(rootFolder, folderName);
            List<string> listOfImgUrls = UploadImageFilesOnServer(images, path, imageFolders);
            return listOfImgUrls;
        }

        public void DeleteOldImages(string rootFolder, int annoucementId)
        {
            string annoucementIdImageFolder = GetImagesFolderPath(rootFolder, $"{annoucementId}");

            DeleteFolder(annoucementIdImageFolder);
        }

        private string GetImagesFolderPath(string rootFolder, string id)
        {
            return Path.Combine(rootFolder, "images", id);
        }

        public List<string> UploadImageFilesOnServer(List<Image> annoucementPhotoFiles, string annoucementIdImageFolder, List<ImageFolderConfig> folders)
        {
            var listOfImgUrls = new List<string>();

            CreateDirectories(annoucementIdImageFolder, folders.Select(x => x.FolderName).ToArray());

            for (int i = 0; i < annoucementPhotoFiles.Count; i++)
            {
                Image imageFile = annoucementPhotoFiles[i];
                string guidFileName = Guid.NewGuid().ToString() + ".jpg";
                folders.ForEach(f =>
                {
                    string filePath = Path.Combine(annoucementIdImageFolder, f.FolderName, guidFileName);
                    SaveResizedImages(imageFile, filePath, f.Resolution);
                });

                listOfImgUrls.Add(guidFileName);
            }

            return listOfImgUrls;
        }

        private void CreateDirectories(string annoucementIdImageFolder, string[] folders)
        {
            CreateDirectory(annoucementIdImageFolder);
            foreach (var folder in folders)
            {
                var folderName = Path.Combine(annoucementIdImageFolder, folder);
                CreateDirectory(folderName);
            }
        }

        private static string GetFullPath(string folder, string guidFileName)
        {
            return Path.Combine(folder, guidFileName);
        }

        public void SaveResizedImages(Image imageFromForm, string filePath, int size)
        {
            Image imageResized;

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                imageResized = _imageFileProcessor.ResizeImage(imageFromForm, new Size(size, size));
                System.Drawing.Imaging.ImageFormat format = System.Drawing.Imaging.ImageFormat.Jpeg;
                imageResized.Save(fileStream, format);
            }
        }

        private void CreateDirectory(string directoryName)
        {
            if (!Directory.Exists(directoryName))
            {
                Directory.CreateDirectory(directoryName);
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
                catch (Exception)
                {
                    throw new Exception($"Could not delete Folder with images on {imagesPath}");
                }
            }
        }
    }
}