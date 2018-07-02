using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace app.Controllers
{
  [Route("/")]
  public class HomeController : Controller
  {
    private readonly ILogger _logger;

    public HomeController(ILogger<HomeController> logger)
    {
      _logger = logger;
    }

    // GET api/values
    [HttpGet]
    public IActionResult Get()
    {
      return new OkObjectResult("ready");
    }

    // POST api/values
    [HttpPost]
    public IActionResult Post([FromBody]string value)
    {
      _logger.LogInformation("Hello, world!");
      return new OkObjectResult(new { a = "ok" });
    }
  }
}
