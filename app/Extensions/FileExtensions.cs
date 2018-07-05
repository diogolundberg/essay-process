using System;
using System.Diagnostics;
using System.IO;

namespace app.Extensions
{
  public class FileExtensions
  {

    public static string ExecShell(string program, string args)
    {
      Process p = new Process();
      p.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
      p.StartInfo.UseShellExecute = false;
      p.StartInfo.RedirectStandardOutput = true;
      p.StartInfo.RedirectStandardError = true;
      p.StartInfo.CreateNoWindow = true;
      p.StartInfo.FileName = program;
      p.StartInfo.Arguments = args;
      p.Start();
      p.WaitForExit();

      string output = p.StandardOutput.ReadToEnd();
      string err = p.StandardError.ReadToEnd();
      return output;
    }
  }
}
