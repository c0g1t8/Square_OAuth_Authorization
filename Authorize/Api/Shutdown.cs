using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Authorize.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class Shutdown : ControllerBase
    {
        private readonly IHostApplicationLifetime applicationLifetime;
        private readonly ILogger<Shutdown> log;

        public Shutdown(IHostApplicationLifetime applicationLifetime, ILogger<Shutdown> log)
        {
            this.applicationLifetime = applicationLifetime;
            this.log = log;
        }

        /// <summary>
        /// shuts down the web application
        /// </summary>
        [HttpGet]
        public void GetShutdown()
        {
            applicationLifetime.StopApplication();
        }
    }
}
