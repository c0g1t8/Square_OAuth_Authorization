﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthorizationProxy.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class Shutdown : ControllerBase
    {
        private readonly IHostApplicationLifetime lifetime;
        private readonly ILogger<Shutdown> log;

        public Shutdown(
            IHostApplicationLifetime lifetime,
            ILogger<Shutdown> log)
        {
            this.lifetime = lifetime;
            this.log = log;
        }

        [HttpGet]
        public void ShutdownApplication()
        {
            lifetime.StopApplication();
        }
    }
}
