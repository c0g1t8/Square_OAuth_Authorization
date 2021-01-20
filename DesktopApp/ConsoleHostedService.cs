using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace DesktopApp
{
    internal class ConsoleHostedService : IHostedService
    {
        private readonly ILogger<ConsoleHostedService> logger;
        private readonly IHostApplicationLifetime appLifetime;
        private int? exitCode;

        public ConsoleHostedService(
            ILogger<ConsoleHostedService> logger,
            IHostApplicationLifetime appLifetime)
        {
            this.logger = logger;
            this.appLifetime = appLifetime;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            logger.LogDebug($"Starting with arguments: {string.Join(" ", Environment.GetCommandLineArgs())}");

            appLifetime.ApplicationStarted.Register(() =>
            {
                Task.Run(async () =>
                {
                    try
                    {
                        logger.LogInformation("Hello World!");

                        // Simulate real work is being done
                        await Task.Delay(1000);

                        exitCode = 0;
                    }
                    catch (Exception ex)
                    {
                        logger.LogError(ex, "Unhandled exception!");
                        exitCode = 1;
                    }
                    finally
                    {
                        // Stop the application once the work is done
                        appLifetime.StopApplication();
                    }
                });
            });

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            logger.LogDebug($"Exiting with return code: {exitCode}");

            // Exit code may be null if the user cancelled via Ctrl+C/SIGTERM
            Environment.ExitCode = exitCode.GetValueOrDefault(-1);
            return Task.CompletedTask;
        }
    }
}