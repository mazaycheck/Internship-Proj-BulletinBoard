using Microsoft.Extensions.Options;
using System.Collections.Generic;

namespace Baraholka.Utilities
{
    public class ImageFolderConfigAccessor : IImageFolderConfigAccessor
    {
        public readonly List<ImageFolderConfig> Folders;

        public ImageFolderConfigAccessor(IOptions<List<ImageFolderConfig>> config)
        {
            Folders = config.Value;
        }

        public List<ImageFolderConfig> GetFolderConfigs() => Folders;
    }
}