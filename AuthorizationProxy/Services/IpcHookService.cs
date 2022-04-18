using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Pipes;
using System.Linq;
using System.Runtime.Versioning;
using System.Threading.Tasks;

[assembly: SupportedOSPlatform("windows")]
namespace AuthorizationProxy.Services
{
    public class IpcHookService : IDisposable
    {
        AnonymousPipeClientStream clientStream = null;
        private readonly ILogger<IpcHookService> logger;

        public IpcHookService(
            IConfiguration config,
            ILogger<IpcHookService> logger)
        {
            this.logger = logger;

            string handle = config["handle"];
            if (handle != null)
            {
                logger.LogInformation($"Connecting to server handle : {handle}");
                try
                {
                    this.clientStream = new AnonymousPipeClientStream(PipeDirection.Out, handle);
                }
                catch (Exception ex)
                {
                    logger.LogCritical($"Unable to connect to server handle : {ex.Message}");
                }
            }
        }

        public void Dispose()
        {
            clientStream?.Dispose();
        }

        public async Task SendMessageAsync(string message)
        {
            if (clientStream != null)
            {
                using (StreamWriter sw = new StreamWriter(clientStream))
                {
                    sw.AutoFlush = true;
                    await sw.WriteLineAsync(message);
                    clientStream.WaitForPipeDrain();
                }
            }
        }
    }
}
