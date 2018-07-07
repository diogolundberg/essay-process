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
  public class Barcode
  {
    const string MAGICK = "magick";
    private Paths Paths { get; }

    public Barcode(IOptions<Paths> paths) => Paths = paths.Value;

    public string Run(string fileName)
    {
      string black = Path.Combine(Paths.Black, Path.GetFileName(fileName));
      MAGICK.Exec($"{fileName} -level 100,5000,1 -threshold 70% {black}");
      return black;
    }
  }
}
