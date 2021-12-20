using System.Net.Http;
using System.Text;
using Newtonsoft.Json;

namespace Elsa.SKS.Backend.IntegrationTests.Extensions
{
    public static class ObjectExtensions
    {
        public static HttpContent ToJsonContent(this object obj)
        {
            var json = JsonConvert.SerializeObject(obj);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            return content;
        }
    }
}