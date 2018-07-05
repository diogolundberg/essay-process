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
  public class Process
  {
    const string MAGICK = "magick";
    private Paths Paths {get;}

    public Process(IOptions<Paths> paths) => Paths = paths.Value;

    public bool Run(string fileName)
    {
      string source = Path.Combine(Paths.Source, fileName);
      string resized = Path.Combine(Paths.Resized, fileName);
      string aligned = Path.Combine(Paths.Aligned, fileName);
      string black = Path.Combine(Paths.Black, fileName);
      string cropped = Path.Combine(Paths.Cropped, fileName);

      MAGICK.Exec($"convert -resize 1070 {source} {resized}");
      MAGICK.Exec($"{resized} -verbose -deskew 40% -trim {aligned}");
      MAGICK.Exec($"{aligned} -level 100,5000,1 -threshold 70% {black}");
      MAGICK.Exec($"convert -crop 980x1040+50+70 {black} {cropped}");

      return true;
    }
  }
}
