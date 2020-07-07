using Microsoft.AspNetCore.Mvc;

namespace DFC.Compui.Subscriptions.Pkg.Webhook.Controllers
{
    public class SomeRandomController : Controller
    {
        [HttpGet("Index")]
        public IActionResult Index()
        {
            return new BadRequestObjectResult("Test");
        }
    }
}
