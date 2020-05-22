using Microsoft.Extensions.Configuration;

namespace Baraholka.Services
{
    public class SecurityKeyProvider : ISecurityKeyProvider
    {
        private readonly IConfiguration _configuration;

        public SecurityKeyProvider(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GetKey()
        {
            return _configuration.GetSection("AppSettings:Token").Value;
        }
    }
}