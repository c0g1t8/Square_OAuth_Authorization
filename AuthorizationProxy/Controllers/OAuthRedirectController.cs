using AuthorizationProxy.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using System.Web;

namespace AuthorizationProxy.Controllers
{
    [Route("oauth-redirect")]
    [ApiController]
    public class OAuthRedirectController : ControllerBase
    {
        private readonly IpcHookService hookService;
        private readonly IServiceScopeFactory serviceScopeFactory;
        private readonly IConfiguration config;
        private readonly ILogger<OAuthRedirectController> logger;

        public OAuthRedirectController(
            IpcHookService hookService,
            IServiceScopeFactory serviceScopeFactory,
            IConfiguration config,
            ILogger<OAuthRedirectController> logger)
        {
            this.hookService = hookService;
            this.serviceScopeFactory = serviceScopeFactory;
            this.config = config;
            this.logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetSquareResponseAsync()
        {
            logger.LogInformation("OAuth Redirect - process response");

            var queryString = HttpContext.Request.QueryString.Value;
            var dict = HttpUtility.ParseQueryString(queryString);
            var json = JsonSerializer.Serialize(dict.AllKeys.ToDictionary(k => k, k => dict[k]));

            //*** send response to process that spawned the proxy server
            await hookService.SendMessageAsync(json);

            //*** run a background task that will stop the proxy server after this function returns
            // https://docs.microsoft.com/en-us/aspnet/core/performance/performance-best-practices?view=aspnetcore-3.1#do-not-capture-services-injected-into-the-controllers-on-background-threads
            _ = Task.Run(async () =>
            {
                int delay;
                if (!int.TryParse(config["AutoShutdownTime"], out delay))
                {
                    delay = 1000;
                }
                await Task.Delay(delay);

                using (var scope = serviceScopeFactory.CreateScope())
                {
                    var lifetime = scope.ServiceProvider.GetRequiredService<IHostApplicationLifetime>();
                    lifetime.StopApplication();
                }
            });

            //*** respond with pre-built html page
            return new ContentResult
            {
                ContentType = "text/html",
                StatusCode = (int)HttpStatusCode.OK,
                Content = System.IO.File.ReadAllText(@"OAuthRedirect.html")
            };
        }
    }
}
