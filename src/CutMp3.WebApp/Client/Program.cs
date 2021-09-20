using CutMp3.Application.Services;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

namespace CutMp3.WebApp.Client
{
    public static class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);

            builder.RootComponents.Add<App>("#app");

            builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
            builder.Services.AddScoped<ToastService>();

            await builder.Build().RunAsync();
        }
    }
}
