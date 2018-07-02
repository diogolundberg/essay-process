using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace app.Controllers
{
  [Route("/")]
  public class MessagesController : Controller
  {
    [HttpGet]
    public IActionResult Get()
    {
      return new OkObjectResult("ready");
    }

    [HttpPost]
    public IActionResult Post([FromBody]dynamic value)
    {
      var info = value.Records.First.s3;

      return new OkObjectResult(new
      {
        bucket = info.bucket.name,
        key = info["object"].key,
      });
    }
  }
}
