using System;
using System.Configuration;
using System.Net.Http;
using System.Threading.Tasks;
using Elsa.SKS.Frontend.Configuration;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Elsa.SKS.Frontend
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");
            ConfigureServices(builder);
            await builder.Build().RunAsync();
        }

        private static void ConfigureServices(WebAssemblyHostBuilder builder)
        {
            var services = builder.Services;
            
            services.AddScoped(sp => new HttpClient
            {
                BaseAddress = new Uri(builder.HostEnvironment.BaseAddress)
            });
            
            services.AddSingleton(sp =>
            {
                var config = sp.GetService<IConfiguration>();
                string appUrl = builder.Configuration.GetValue<string>("App:AppUrl");
                var appConfiguration = new AppConfiguration { AppUrl = appUrl };
                return appConfiguration;
            });
        }
    }
}

