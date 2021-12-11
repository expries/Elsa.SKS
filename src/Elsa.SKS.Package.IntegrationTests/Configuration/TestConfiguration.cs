using Microsoft.Extensions.Configuration;

namespace Elsa.SKS.Package.IntegrationTests.Configuration
{
    public static class TestConfiguration
    {
        public static string BaseUrl => Configuration.GetSection("BaseUrl").Value;
        
        private static readonly IConfiguration Configuration = GetConfiguration();

        private static IConfiguration GetConfiguration()
        {
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("Configuration/testsettings.json", true, true)
                .AddEnvironmentVariables()
                .Build();

            return configuration;
        }
    }
}