using AuthorizationProxy.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using System.Web;

namespace AuthorizationProxy.Controllers
{
    [Route("oauth-redirect")]
    [ApiController]
    public class OAuth_Redirect : ControllerBase
    {
        private readonly AuthorizationResponseService responseService;
        private readonly ILogger<OAuth_Redirect> log;

        public OAuth_Redirect(
            AuthorizationResponseService responseService,
            ILogger<OAuth_Redirect> log)
        {
            this.responseService = responseService;
            this.log = log;
        }

        [HttpGet]
        public IActionResult GetSquareResponse()
        {
            var queryString = HttpContext.Request.QueryString.Value;
            var dict = HttpUtility.ParseQueryString(queryString);
            var json = JsonSerializer.Serialize(dict.AllKeys.ToDictionary(k => k, k => dict[k]));

            responseService.AuthorizationResponse = json;

            return Ok("Authorization process completed.");
        }
    }
}
