using System.Collections.Generic;

namespace Baraholka.Utilities
{
    public interface IImageFolderFactory
    {
        List<ImageFolderConfig> GetFolderConfigs();
    }
}