using Microsoft.AspNetCore.Mvc;

namespace OnlineElectronicsStore.Controllers
{
    [ApiController]
    [Route("/")]
    public class HealthController : ControllerBase
    {
        [HttpGet]
        public IActionResult Index()
        {
            return Ok("🚀 API is working! Welcome to the Online Electronics Store.");
        }
    }
}
