using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Drawing;

namespace Baraholka.Utilities
{
    public interface IImageFileProcessor
    {
        List<Image> ConvertIFormFileToImage(List<IFormFile> formFiles);

        Image ResizeImage(Image imgToResize, Size size);
    }
}