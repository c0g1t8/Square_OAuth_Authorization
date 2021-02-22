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
    public class AuthorizationRequestController : ControllerBase
    {
        private readonly IConfiguration config;
        private readonly ILogger<AuthorizationRequestController> logger;

        public AuthorizationRequestController(
            IConfiguration config,
            ILogger<AuthorizationRequestController> logger)
        {
            this.config = config;
            this.logger = logger;
        }

        [HttpGet]
        public IActionResult GetSquareAuthorization()
        {
            logger.LogInformation("Application authorization request - redirect to Square OAuth service");

            // need authorization to allow merchants to receive payments
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
            if (string.IsNullOrEmpty(squareApplicationId))
            {
                throw new ArgumentException("Missing square application Id");
            }

            if (string.IsNullOrEmpty(scope))
            {
                throw new ArgumentException("Scope must be specified");
            }

            string authorizationBaseUrl = squareEnvironment.Equals("Production", StringComparison.OrdinalIgnoreCase) ?
                "https://connect.squareup.com/" : "https://connect.squareupsandbox.com/";

            string url = authorizationBaseUrl + "oauth2/authorize";
            url += $"?client_id={squareApplicationId}";
            url += $"&scope={scope}";
            // Based on https://developer.squareup.com/docs/oauth-api/best-practices#session
            // Force user to explicitly login for production. Omit for sandbox
            // https://developer.squareup.com/docs/oauth-api/walkthrough#step-2-link-to-the-square-authorization-page
            url += squareEnvironment == "Production" ? "&session=false" : "";

            return url;
        }
    }
}
