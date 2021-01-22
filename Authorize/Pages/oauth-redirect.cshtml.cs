using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using System.Web;
using Authorize.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace Authorize.Pages
{
    public class oauth_redirectModel : PageModel
    {
        private readonly AuthorizationResponseService responseService;
        private readonly ILogger<oauth_redirectModel> log;

        public oauth_redirectModel(
            AuthorizationResponseService responseService,
            ILogger<oauth_redirectModel> log)
        {
            this.responseService = responseService;
            this.log = log;
        }

        public void OnGet()
        {
            var queryString = HttpContext.Request.QueryString.Value;
            var dict = HttpUtility.ParseQueryString(queryString);
            var json = JsonSerializer.Serialize(dict.AllKeys.ToDictionary(k => k, k => dict[k]));

            responseService.AuthorizationResponse = json;
        }
    }
}
