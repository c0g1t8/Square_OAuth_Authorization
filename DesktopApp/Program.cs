using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace DesktopApp
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var hostBuilder = Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostctx, services) =>
                {
                    services.AddHostedService<ConsoleHostedService>();
                });

            await hostBuilder.RunConsoleAsync();
        }
    }
}
