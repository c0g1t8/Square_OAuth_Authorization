using AuthorizationProxy.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthorizationProxy.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AuthorizationResponseController : ControllerBase
    {
        private readonly AuthorizationResponseService responseService;
        private readonly ILogger<AuthorizationResponseController> log;

        public AuthorizationResponseController(
            AuthorizationResponseService responseService,
            ILogger<AuthorizationResponseController> log)
        {
            this.responseService = responseService;
            this.log = log;
        }

        [HttpGet]
        public IActionResult GetSquareRespoonse()
        {
            return Ok(responseService.AuthorizationResponse);
        }
    }
}
