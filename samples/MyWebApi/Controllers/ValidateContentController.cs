using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MyWebApi.ContentHashValidation;

namespace MyWebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ValidateContentController : ControllerBase
    {
        [HttpPost("validated")]
        [ValidateContentHash]
        public ActionResult Validated()
        {
            return Ok();
        }

        [HttpGet("not-validated")]
        public ActionResult NotValidated()
        {
            return Ok();
        }
    }
}
