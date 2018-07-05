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
    private UploadService UploadService { get; }
    private ProcessImage ProcessImage { get; }
    private ImagesPath ImagesPath { get; }
    public MessagesController(UploadService uploadService,
      ProcessImage processImage,
      IOptions<ImagesPath> imagesPath)
    {
      UploadService = uploadService;
      ProcessImage = processImage;
      ImagesPath = imagesPath.Value;
    }

    [HttpPost]
    public IActionResult Post([FromBody]dynamic value)
    {
      string key = value.Records.First.s3["object"].key;
      ImagesPath.CreatePaths();
      if (DownloadFile(key).Result) ProcessImage.Run(key);
      string resultPath = $"{ImagesPath.Cropped}/{key}";
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
        response.Content.ReadAsFileAsync($"{ImagesPath.Source}/{key}");
      });

      return System.IO.File.Exists($"{ImagesPath.Source}/{key}");
    }
  }
}
