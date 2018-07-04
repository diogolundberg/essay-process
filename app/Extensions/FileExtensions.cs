using System;
using System.Diagnostics;
using System.IO;

namespace app.Extensions
{
  public class FileExtensions
  {
    public static string FileExtension(string fileName)
    {
      int fileExtPos = fileName.LastIndexOf(".");
      if (fileExtPos >= 0)
      {
        fileName = fileName.Substring(fileExtPos + 1);
      }
      return fileName;
    }

    public static void DeleteFile(string file)
    {
      if (File.Exists(file))
      {
        File.Delete(file);
      }
    }

    public static string ExecShell(string program, string args, bool waitForExit)
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

      if (waitForExit)
      {
        p.WaitForExit();
      }

      string output = p.StandardOutput.ReadToEnd();
      string err = p.StandardError.ReadToEnd();
      //            if (!err.Trim().Equals("") && err != null)
      //            {
      //                throw new Exception(err);
      //            }
      //            else if (!output.Equals("") && output != null)
      //            {
      return output;
      //            }
      //            return null;
    }
    public static string RemoveFileExtension(string fileName)
    {
      int fileExtPos = fileName.LastIndexOf(".");
      if (fileExtPos >= 0)
      {
        fileName = fileName.Substring(0, fileExtPos);
      }
      return fileName;
    }
  }
}
