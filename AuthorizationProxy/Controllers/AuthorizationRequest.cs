using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthorizationProxy.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AuthorizationRequest : ControllerBase
    {
        private readonly IConfiguration config;
        private readonly ILogger<AuthorizationRequest> logger;

        public AuthorizationRequest(
            IConfiguration config,
            ILogger<AuthorizationRequest> logger)
        {
            this.config = config;
            this.logger = logger;
        }

        [HttpGet]
        public IActionResult GetSquareAuthorization()
        {
            // need authorization to allow merchants to recieve payments
            string url = AuthorizationUrl(
                config["SquareEnvironment"],
                config["SquareApplicationId"],
                "MERCHANT_PROFILE_READ+ORDERS_WRITE+PAYMENTS_WRITE");

            return Redirect(url);
        }

        /// <summary>
        /// Builds a URL for authorizing application
        /// </summary>
        /// <remarks><a href="https://developer.squareup.com/docs/oauth-api/how-oauth-works"/></remarks>
        /// <param name="squareEnvironment">environment</param>
        /// <param name="squareApplicationId">client_id</param>
        /// <param name="scope">permissions</param>
        /// <returns>URL to authorize application access to merchant </returns>
        static string AuthorizationUrl(
            string squareEnvironment,
            string squareApplicationId,
            string scope)
        {
            string authorizationBaseUrl = squareEnvironment == "Production" ?
                "https://connect.squareup.com/" : "https://connect.squareupsandbox.com/";

            // based on https://developer.squareup.com/docs/oauth-api/best-practices#session
            // force user to explicitly login for production. Omit for sandbox
            // https://developer.squareup.com/docs/oauth-api/walkthrough#step-2-link-to-the-square-authorization-page
            string session = squareEnvironment == "Production" ? "&session=false" : "";

            var sb = new StringBuilder();
            sb.Append(authorizationBaseUrl);
            sb.Append("oauth2/authorize?client_id=");
            sb.Append(squareApplicationId);
            sb.Append("&scope=");
            sb.Append(scope);
            sb.Append(session);         // this will an empty string for Sandbox
            string url = sb.ToString();

            return url;
        }
    }
}
