using Microsoft.AspNetCore.Mvc;

namespace Security.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        [HttpGet()]
        public ActionResult<string> Get()
        {
            return "Identity Server is working.";
        }
    }
}