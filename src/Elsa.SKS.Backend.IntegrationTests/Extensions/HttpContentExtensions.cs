using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Elsa.SKS.Backend.IntegrationTests.Extensions
{
    public static class HttpContentExtensions
    {
        public static async Task<T> ToJsonAsync<T>(this HttpContent content)
        {
            var stringContent = await content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<T>(stringContent);
        }
    }
}