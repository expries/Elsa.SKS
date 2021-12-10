using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Elsa.SKS.Package.IntegrationTests
{
    public static class TestConfiguration
    {
        public static readonly string BaseUrl = GetBaseUrl();

        private static string GetBaseUrl()
        {
            using var file = File.OpenText("Configuration/testsettings.json");
            var reader = new JsonTextReader(file);
            var jObject = JObject.Load(reader);
            var baseUrl = jObject["BaseUrl"]?.Value<string>() ?? string.Empty;
            return baseUrl;
        }
    }
}