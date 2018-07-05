using System;
using System.Diagnostics;
using System.IO;

namespace app.Extensions
{
  public static class StringExtensions
  {

    public static string Exec(this String arguments, string fileName)
    {
      Process p = new Process();
      p.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
      p.StartInfo.UseShellExecute = false;
      p.StartInfo.RedirectStandardOutput = true;
      p.StartInfo.RedirectStandardError = true;
      p.StartInfo.CreateNoWindow = true;
      p.StartInfo.FileName = fileName;
      p.StartInfo.Arguments = arguments;
      p.Start();
      p.WaitForExit();

      string output = p.StandardOutput.ReadToEnd();
      string err = p.StandardError.ReadToEnd();
      return output;
    }
  }
}
