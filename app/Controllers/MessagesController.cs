﻿using app.Extensions;
using app.Options;
using app.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace app.Controllers
{
  [Route("/[controller]")]
  [Produces("application/json")]
  public class MessagesController : Controller
  {
    private Upload Upload { get; }
    private Process Process { get; }
    private Paths Paths { get; }
    private Download Download { get; }

    public MessagesController(IOptions<Paths> paths,
      Process process,
      Download download,
      Upload upload)
    {
      Paths = paths.Value;
      Download = download;
      Process = process;
      Upload = upload;
    }

    [HttpPost]
    public IActionResult Post([FromBody]dynamic value)
    {
      Paths.CreatePaths();
      string key = value.Records.First.s3["object"].key;
      string resultPath = $"{Paths.Cropped}/{key}";
      if (Download.Run(key).Result) Process.Run(key);
      return new OkObjectResult(Upload.Run(resultPath, $"ready/{key}").Result);
    }
  }
}
