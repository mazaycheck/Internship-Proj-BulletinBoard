using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Drawing;

namespace Baraholka.Utilities
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

            float percent;
            float percentW;
            float percentH;

            percentW = (size.Width / (float)sourceWidth);
            percentH = (size.Height / (float)sourceHeight);

            if (percentH < percentW)
                percent = percentH;
            else
                percent = percentW;

            int destWidth = (int)(sourceWidth * percent);
            int destHeight = (int)(sourceHeight * percent);

            Bitmap b = new Bitmap(destWidth, destHeight);
            Graphics g = Graphics.FromImage((Image)b);

            g.DrawImage(imgToResize, 0, 0, destWidth, destHeight);
            g.Dispose();

            return (Image)b;
        }
    }
}