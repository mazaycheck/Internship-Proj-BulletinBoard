using Microsoft.Extensions.Options;
using System.Collections.Generic;

namespace Baraholka.Utilities
{
    public class ImageFolderFactory : IImageFolderFactory
    {
        public readonly List<ImageFolderConfig> Folders;

        public ImageFolderFactory(IOptions<List<ImageFolderConfig>> config)
        {
            Folders = config.Value;
        }

        public List<ImageFolderConfig> GetFolderConfigs() => Folders;
    }
}