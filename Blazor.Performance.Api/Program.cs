using Blazor.Performance.Api.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Threading.Tasks;

namespace Blazor.Performance.Api
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            IHost host = CreateHostBuilder(args).Build();

            // Create a new scope
            using (var scope = host.Services.CreateScope())
            {
                // Get the DbContext instance
                var confService = scope.ServiceProvider.GetRequiredService<ConferencesService>();
                var contributionsService = scope.ServiceProvider.GetRequiredService<ContributionsService>();
                var speakerService = scope.ServiceProvider.GetRequiredService<SpeakerService>();

                //Do the migration asynchronously
                await confService.InitAsync();
                await contributionsService.InitAsync();
                await speakerService.InitAsync();
            }

            // Run the WebHost, and start accepting requests
            // There's an async overload, so we may as well use it
            await host.RunAsync();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
