using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Amazon.S3;
using Amazon.S3.Model;
using app.Extensions;
using app.Models;
using app.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace app.Controllers
{
  [Route("/")]
  public class MessagesController : Controller
  {

    private UploadService UploadService { get; set; }

    public MessagesController(UploadService uploadService)
    {
      this.UploadService = uploadService;
    }

    [HttpGet]
    public IActionResult Get()
    {
      return new OkObjectResult("ready");
    }

    [HttpPost]
    public IActionResult Post([FromBody]dynamic value)
    {
      var info = value.Records.First.s3;
      string key = info["object"].key;
      string bucket = info.bucket.name;

      if (DownloadFile($"https://s3-sa-east-1.amazonaws.com/{bucket}/{key}", $"/Users/dclundberg/tmp/originais/{key}").Result)
      {

        var processoSeletivo = new ProcessoSeletivo();
        processoSeletivo.CaminhoImagens = "/Users/dclundberg/tmp/originais";
        processoSeletivo.CaminhoImagensProcessadas = "/Users/dclundberg/tmp/processadas";
        processoSeletivo.CaminhoImagensRecortadas = "/Users/dclundberg/tmp/recortadas";
        processoSeletivo.CaminhoImagensRejeitadas = "/Users/dclundberg/tmp/rejeitadas";
        processoSeletivo.CaminhoImagensCorrecao = "/Users/dclundberg/tmp/correcao";

        var process = new ProcessImage(processoSeletivo);
        process.Process(key);
      }

      return new OkObjectResult(UploadService.Run(bucket, $"/Users/dclundberg/tmp/recortadas/c_red_{key}", key).Result);
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
