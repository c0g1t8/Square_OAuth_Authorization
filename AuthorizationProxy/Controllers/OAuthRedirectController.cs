using AuthorizationProxy.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
        private readonly AuthorizationResponseService responseService;
        private readonly ILogger<OAuthRedirectController> log;

        public OAuthRedirectController(
            AuthorizationResponseService responseService,
            ILogger<OAuthRedirectController> log)
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

            return new ContentResult
            {
                ContentType = "text/html",
                StatusCode = (int)HttpStatusCode.OK,
                Content = System.IO.File.ReadAllText(@"OAuthRedirect.html")
            };
        }
    }
}
