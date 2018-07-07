using app.Extensions;
using app.Options;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.IO;

namespace app.Services
{
  public class Resize
  {
    const string MAGICK = "magick";
    private Paths Paths { get; }

    public Resize(IOptions<Paths> paths) => Paths = paths.Value;

    public string Run(string fileName)
    {
      string resized = Path.Combine(Paths.Resized, Path.GetFileName(fileName));
      MAGICK.Exec($"convert -crop 980x1040+50+70 {fileName} {resized}");
      return resized;
    }
  }
}
