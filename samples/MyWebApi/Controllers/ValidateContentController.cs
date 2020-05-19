using Microsoft.AspNetCore.Mvc;
using ContentHashValidation;

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
