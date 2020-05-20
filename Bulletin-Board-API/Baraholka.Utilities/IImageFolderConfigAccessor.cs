using System.Collections.Generic;

namespace Baraholka.Utilities
{
    public interface IImageFolderConfigAccessor
    {
        List<ImageFolderConfig> GetFolderConfigs();
    }
}