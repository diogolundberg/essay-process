using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace app.Extensions
{
  public static class HttpContentExtensions
  {
    public static Task ReadAsFileAsync(this HttpContent content, string filename)
    {
      string path = Path.GetFullPath(filename);

      FileStream stream = null;
      try
      {
        stream = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.None);
        return content.CopyToAsync(stream).ContinueWith((_task) => stream.Close());
      }
      catch
      {
        if (stream != null) stream.Close();
        throw;
      }
    }
  }
}
