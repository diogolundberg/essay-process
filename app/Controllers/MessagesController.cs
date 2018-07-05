using app.Extensions;
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
    private Upload UploadService { get; }
    private Process Process { get; }
    private Paths Paths { get; }
    public MessagesController(Upload uploadService,
      Process process,
      IOptions<Paths> paths)
    {
      UploadService = uploadService;
      Process = process;
      Paths = paths.Value;
    }

    [HttpPost]
    public IActionResult Post([FromBody]dynamic value)
    {
      string key = value.Records.First.s3["object"].key;
      Paths.CreatePaths();
      if (DownloadFile(key).Result) Process.Run(key);
      string resultPath = $"{Paths.Cropped}/{key}";
      return new OkObjectResult(UploadService.Run(resultPath, $"ready/{key}").Result);
    }

    public async Task<bool> DownloadFile(string key)
    {
      HttpClient client = new HttpClient();
      string url = $"https://s3-sa-east-1.amazonaws.com/educat-images/{key}";

      await client.GetAsync(url).ContinueWith((requestTask) =>
      {
        HttpResponseMessage response = requestTask.Result;
        response.EnsureSuccessStatusCode();
        response.Content.ReadAsFileAsync($"{Paths.Source}/{key}");
      });

      return System.IO.File.Exists($"{Paths.Source}/{key}");
    }
  }
}
