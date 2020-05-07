using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;

namespace WebApplication2.Helpers
{
    public class ImageFileProcessor
    {
        public static List<Image> ConvertIFormFileToImage(List<IFormFile> formFiles)
        {
            List<Image> list = new List<Image>();
            foreach (var formFile in formFiles)
            {
                Image imageRaw;
                using (var stream = formFile.OpenReadStream())
                {
                    imageRaw = Image.FromStream(stream);
                }
                list.Add(imageRaw);
            }
            return list;
        }
        
        public static List<string> UploadFilesOnServer(List<Image> annoucementPhotoFiles, string annoucementIdImageFolder)
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
                
                SaveImageResized(imageFile, largeImageFilePath, 1000, 1000);
                SaveImageResized(imageFile, mediumImageFilePath, 500, 500);
                SaveImageResized(imageFile, smallImageFilePath, 200, 200);

                listOfImgUrls.Add(guidFileName);
            }

            return listOfImgUrls;
        }
      
        public static void SaveImageResized(Image imageFromForm, string filePath, int width, int height)
        {
            Image imageResized;

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                imageResized = ImageProcessor.resizeImage(imageFromForm, new Size(width, height));
                System.Drawing.Imaging.ImageFormat format = System.Drawing.Imaging.ImageFormat.Jpeg;
                imageResized.Save(fileStream, format);
            }
        }
     
        private static void SaveImageOriginal(Image imageFromForm, string largeImageFilePath)
        {
            System.Drawing.Imaging.ImageFormat format = System.Drawing.Imaging.ImageFormat.Jpeg;

            using (var fileStream = new FileStream(largeImageFilePath, FileMode.Create))
            {             
                imageFromForm.Save(fileStream, format);
            }
        }

        private static void CreateDirectories(params string[] directories)
        {
            foreach (var dir in directories)
            {
                if (!Directory.Exists(dir))
                {
                    Directory.CreateDirectory(dir);
                }
            }
        }

        public static void DeleteFolder(string imagesPath)
        {            
            if (Directory.Exists(imagesPath)) { 
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
