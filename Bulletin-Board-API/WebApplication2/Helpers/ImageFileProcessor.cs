using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;

namespace WebApplication2.Helpers
{
    public class ImageFileProcessor : IImageFileProcessor
    {
        public List<Image> ConvertIFormFileToImage(List<IFormFile> formFiles)
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

        public Image ResizeImage(Image imgToResize, Size size)
        {
            int sourceWidth = imgToResize.Width;
            int sourceHeight = imgToResize.Height;

            float nPercent = 0;
            float nPercentW = 0;
            float nPercentH = 0;

            nPercentW = ((float)size.Width / (float)sourceWidth);
            nPercentH = ((float)size.Height / (float)sourceHeight);

            if (nPercentH < nPercentW)
                nPercent = nPercentH;
            else
                nPercent = nPercentW;

            int destWidth = (int)(sourceWidth * nPercent);
            int destHeight = (int)(sourceHeight * nPercent);

            Bitmap b = new Bitmap(destWidth, destHeight);
            Graphics g = Graphics.FromImage((Image)b);
            //g.InterpolationMode = InterpolationMode.HighQualityBicubic;

            g.DrawImage(imgToResize, 0, 0, destWidth, destHeight);
            g.Dispose();

            return (Image)b;
        }

        public List<string> UploadFilesOnServer(List<Image> annoucementPhotoFiles, string annoucementIdImageFolder)
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

        public void SaveImageResized(Image imageFromForm, string filePath, int width, int height)
        {
            Image imageResized;

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                imageResized = ResizeImage(imageFromForm, new Size(width, height));
                System.Drawing.Imaging.ImageFormat format = System.Drawing.Imaging.ImageFormat.Jpeg;
                imageResized.Save(fileStream, format);
            }
        }

        private void SaveImageOriginal(Image imageFromForm, string largeImageFilePath)
        {
            System.Drawing.Imaging.ImageFormat format = System.Drawing.Imaging.ImageFormat.Jpeg;

            using (var fileStream = new FileStream(largeImageFilePath, FileMode.Create))
            {
                imageFromForm.Save(fileStream, format);
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