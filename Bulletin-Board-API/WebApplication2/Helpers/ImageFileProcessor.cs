using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using WebApplication2.Models;

namespace WebApplication2.Helpers
{
    public class ImageFileProcessor
    {

        public static Image ConvertIFormFileToImage(IFormFile formFile)
        {
            Image imageRaw;
            using (var stream = formFile.OpenReadStream())
            {
                imageRaw = Image.FromStream(stream);                                
            }            
            return imageRaw;            
        }

        public static List<Image> ConvertListIFormFileToListImage(List<IFormFile> formFiles)
        {
            List<Image> list = new List<Image>();
            foreach (var item in formFiles)
            {
                list.Add(ConvertIFormFileToImage(item));
            }
            return list;
        }


        //public static async Task<List<string>> UploadFilesOnServerAndGetListOfFileNames(List<IFormFile> annoucementPhotoFiles, string annoucementIdImageFolder)
        public static List<string> UploadFilesOnServerAndGetListOfFileNames(List<Image> annoucementPhotoFiles, string annoucementIdImageFolder)
        {
            var listOfImgUrls = new List<string>();
            
            string smallImageFolder = Path.Combine(annoucementIdImageFolder, "small");
            string mediumImageFolder = Path.Combine(annoucementIdImageFolder, "medium");
            string largeImageFolder = Path.Combine(annoucementIdImageFolder, "large");

            CreateDirectories(annoucementIdImageFolder, smallImageFolder, mediumImageFolder, largeImageFolder);

            for (int i = 0; i < annoucementPhotoFiles.Count; i++)
            {
                Image imageFile = annoucementPhotoFiles[i];

                //string fileExtension = Path.GetExtension(imageFile.FileName);
                
                string guidFileName = Guid.NewGuid().ToString() + ".jpg";

                string largeImageFilePath = Path.Combine(largeImageFolder, guidFileName);
                string mediumImageFilePath = Path.Combine(mediumImageFolder, guidFileName);
                string smallImageFilePath = Path.Combine(smallImageFolder, guidFileName);

                //SaveImageOriginal(imageFile, largeImageFilePath);
                SaveImageResized(imageFile, largeImageFilePath, 1000, 1000);
                SaveImageResized(imageFile, mediumImageFilePath, 500, 500);
                SaveImageResized(imageFile, smallImageFilePath, 200, 200);

                listOfImgUrls.Add(guidFileName);
            }

            return listOfImgUrls;
        }



        //public static void SaveImageResized(IFormFile imageFromForm, string filePath, int width, int height)
        public static void SaveImageResized(Image imageFromForm, string filePath, int width, int height)
        {
            Image imageResized;

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                //using (var stream = imageFromForm.OpenReadStream())
                //{
                //    var imageRaw = Image.FromStream(stream);
                //    imageResized = ImageProcessor.resizeImage(imageRaw, new Size(width, height));
                //}

                imageResized = ImageProcessor.resizeImage(imageFromForm, new Size(width, height));

                System.Drawing.Imaging.ImageFormat format = System.Drawing.Imaging.ImageFormat.Jpeg;


                //System.Drawing.Imaging.ImageFormat format;

                //switch (Path.GetExtension(imageFromForm.FileName))
                //{
                //    case "jpg": format = System.Drawing.Imaging.ImageFormat.Jpeg; break;
                //    case "bmp": format = System.Drawing.Imaging.ImageFormat.Bmp; break;
                //    case "png": format = System.Drawing.Imaging.ImageFormat.Png; break;
                //    case "gif": format = System.Drawing.Imaging.ImageFormat.Gif; break;
                //    default: format = System.Drawing.Imaging.ImageFormat.Jpeg; break;
                //}
                imageResized.Save(fileStream, format);
            }

        }

        //private static void SaveImageOriginal(IFormFile imageFromForm, string largeImageFilePath)
        private static void SaveImageOriginal(Image imageFromForm, string largeImageFilePath)
        {

            System.Drawing.Imaging.ImageFormat format = System.Drawing.Imaging.ImageFormat.Jpeg;

            using (var fileStream = new FileStream(largeImageFilePath, FileMode.Create))
            {
                //imageFromForm.CopyTo(fileStream);
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

        public static void DeleteFolderWithAnnoucementPhotos(string imagesPath)
        {
            
            if (Directory.Exists(imagesPath))
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
