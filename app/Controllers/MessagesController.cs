using app.Extensions;
using app.Models;
using app.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
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
    private IConfiguration Configuration { get; }

    public MessagesController(UploadService uploadService, IConfiguration configuration)
    {
      UploadService = uploadService;
      Configuration = configuration;
    }

    [HttpPost]
    public IActionResult Post([FromBody]dynamic value)
    {
      var info = value.Records.First.s3;
      string key = info["object"].key;
      string bucket = info.bucket.name;
      string root = Configuration.Get("contentRoot");
      string source = Path.Combine(root, "tmp/originais");
      string result = Path.Combine(root, "tmp/recortadas");

      if (DownloadFile($"https://s3-sa-east-1.amazonaws.com/{bucket}/{key}", $"{source}/{key}").Result)
      {
        var processoSeletivo = new ProcessoSeletivo();
        processoSeletivo.CaminhoImagens = source;
        processoSeletivo.CaminhoImagensProcessadas = Path.Combine(root, "tmp/processadas");
        processoSeletivo.CaminhoImagensRejeitadas = Path.Combine(root, "tmp/rejeitadas");
        processoSeletivo.CaminhoImagensRecortadas = result;

        var process = new ProcessImage(processoSeletivo);
        process.Process(key);
      }

      string resultPath = Path.Combine(result, $"c_red_{key}");

      return new OkObjectResult(UploadService.Run(bucket, resultPath, $"correcao/{key}").Result);
    }

    public async Task<bool> DownloadFile(string url, string path)
    {
      HttpClient client = new HttpClient();

      await client.GetAsync(url).ContinueWith((requestTask) =>
      {
        HttpResponseMessage response = requestTask.Result;
        response.EnsureSuccessStatusCode();
        response.Content.ReadAsFileAsync(path, true);
      });

      return System.IO.File.Exists(path);
    }
  }
}
