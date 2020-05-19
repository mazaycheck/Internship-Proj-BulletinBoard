using Microsoft.AspNetCore.Hosting;

namespace Baraholka.Services
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
