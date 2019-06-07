using Microsoft.AspNetCore.Mvc;

namespace Pitch.Gateway.Api.Controllers
{
    [Route("/")]
    [ApiController]
    public class ReadinessController : ControllerBase
    {
        // GET /
        [HttpGet]
        public ActionResult Get()
        {
            return Ok();
        }
    }
}
