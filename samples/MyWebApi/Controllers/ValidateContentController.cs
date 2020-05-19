using Microsoft.AspNetCore.Mvc;
using ContentHashValidation;
using InOutLogging;

namespace MyWebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class NoBindingController : ControllerBase
    {
        [HttpPost("nothing")]
        public ActionResult Nothing()
        {
            return Ok();
        }

        [HttpPost("only-hash")]
        [ValidateContentHash]
        [NoContentInOutLogging]
        public ActionResult Only_Hash()
        {
            return Ok();
        }

        [HttpPost("hash-no-content")]
        [ValidateContentHash]
        [NoContentInOutLogging]
        public ActionResult Hash_Without_Content_InOnLogging()
        {
            return Ok();
        }

        [HttpPost("all")]
        [ValidateContentHash]
        public ActionResult Hash_With_InOnLogging()
        {
            return Ok();
        }
    }
}
