using Authorize.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Authorize.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorizationResponse : ControllerBase
    {
        private readonly AuthorizationResponseService responseService;
        private readonly ILogger<AuthorizationResponse> log;

        public AuthorizationResponse(
            AuthorizationResponseService responseService,
            ILogger<AuthorizationResponse> log)
        {
            this.responseService = responseService;
            this.log = log;
        }

        [HttpGet]
        public string GetAuthorizationResponse()
        {
            return responseService.AuthorizationResponse;
        }
    }
}
