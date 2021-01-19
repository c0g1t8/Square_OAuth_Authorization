using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Authorize
{
    class UserAuthorizationRequestor
    {
        private readonly IConfiguration config;
        private readonly ILogger<UserAuthorizationRequestor> log;

        public UserAuthorizationRequestor(IConfiguration config, ILogger<UserAuthorizationRequestor> log)
        {
            this.config = config;
            this.log = log;
        }

        /// <summary>
        /// redirect to square where user will be requested to login and authorize 
        /// </summary>
        public void SendRequest()
        {
            log.LogInformation("Open browser");
            OpenBrowser("http://c0g1t8.com");
        }

        /// <summary>
        /// blocking call listening for response from square
        /// </summary>
        public void StartListening()
        {
            log.LogInformation("Listening started");

            var s = config["CallbackUrls"];

            var host = new WebHostBuilder()
               .UseKestrel()
               .UseUrls(config["CallbackUrls"])
               .UseStartup<Startup>()
               .Build();

            host.Run();
        }

        public void StopListening()
        {
            log.LogInformation("Listening stopped");
        }
        /// <summary>
        /// Opens default browser
        /// </summary>
        /// <remarks>https://brockallen.com/2016/09/24/process-start-for-urls-on-net-core/</remarks>
        /// <param name="url"></param>
        void OpenBrowser(string url)
        {
            try
            {
                Process.Start(url);
            }
            catch
            {
                // hack because of this: https://github.com/dotnet/corefx/issues/10361
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                {
                    url = url.Replace("&", "^&");
                    Process.Start(
                        new ProcessStartInfo("cmd", $"/c start {url}")
                        {
                            CreateNoWindow = true
                        });
                }
                else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                {
                    Process.Start("xdg-open", url);
                }
                else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                {
                    Process.Start("open", url);
                }
                else
                {
                    throw;
                }
            }
        }
    }
}
