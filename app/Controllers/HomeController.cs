using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace app.Controllers
{
  [Route("/")]
  public class HomeController : Controller
  {
    [HttpGet]
    public IActionResult Get() => new OkObjectResult("Ready!");
  }
}
