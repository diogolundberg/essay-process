
using System.Net.Http;
using System.Threading.Tasks;
using app.Extensions;
using app.Options;
using Microsoft.Extensions.Options;

namespace app.Services
{
  public class Download
  {
    private Paths Paths { get; }

    public Download(IOptions<Paths> paths) => Paths = paths.Value;

    public async Task<bool> Run(string key)
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
