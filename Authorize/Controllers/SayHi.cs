using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Authorize.Controllers
{
    public class SayHi : ControllerBase
    {
        [Route("sayhi/{name}")]
        public IActionResult Get(string name)
        {
            return Ok($"Hello {name}");
        }
    }
}
