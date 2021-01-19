using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;

namespace Authorize
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            var config = new ConfigurationBuilder()
                .AddJsonFile("appSettings.json")
                .Build();

            var services = new ServiceCollection();
            ConfigureServices(services, config);
            var serviceProvider = services.BuildServiceProvider();

            // 
            var requestor = serviceProvider.GetService<UserAuthorizationRequestor>();
            requestor.SendRequest();
            requestor.StartListening();
            requestor.StopListening();
        }

        private static void ConfigureServices(IServiceCollection services, IConfiguration config)
        {
            services
                .AddSingleton<IConfiguration>(config)
                .AddLogging(configure =>
                {
                    configure
                    .AddConsole()
                    .AddConfiguration(config.GetSection("Logging"));
                })
                .AddSingleton<UserAuthorizationRequestor>();
        }

    }
}
