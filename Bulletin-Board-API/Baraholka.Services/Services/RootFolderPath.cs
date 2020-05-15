using Microsoft.AspNetCore.Hosting;

namespace Baraholka.Services.Services
{
    public class WebRootFolderPath : IRootFolderPath
    {
        private readonly IWebHostEnvironment _webHost;

        public WebRootFolderPath(IWebHostEnvironment webHost)
        {
            _webHost = webHost;
        }

        public string FolderPath => _webHost.WebRootPath;
    }
}