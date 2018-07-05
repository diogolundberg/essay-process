

using System;
using System.IO;

namespace app.Options
{
  public class ImagesPath
  {
    public void CreatePaths()
    {
      string[] paths = new[] { Source, Resized, Aligned, Black, Cropped };
      Array.ForEach(paths, path => Directory.CreateDirectory(path));
    }

    public string Source { get; set; } = "tmp/source";
    public string Resized{ get; set; } = "tmp/resized";
    public string Aligned{ get; set; } = "tmp/aligned";
    public string Black{ get; set; } = "tmp/black";
    public string Cropped{ get; set; } = "tmp/cropped";
  }
}
