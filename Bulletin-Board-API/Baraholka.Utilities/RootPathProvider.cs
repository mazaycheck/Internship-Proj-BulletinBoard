using Microsoft.AspNetCore.Hosting;

namespace Baraholka.Utilities
{
    public class RootPathProvider : IRootPathProvider
    {
        private readonly IWebHostEnvironment _webHost;

        public RootPathProvider(IWebHostEnvironment webHost)
        {
            _webHost = webHost;
        }

        public string GetRootPath()
        {
            return _webHost.WebRootPath;
        }
    }
}