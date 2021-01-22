using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Authorize.Pages
{
    public class oauth_redirectModel : PageModel
    {
        public void OnGet()
        {
            var q = HttpContext.Request.Query;
        }
    }
}
