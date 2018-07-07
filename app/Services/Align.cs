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
  public class Align
  {
    const string MAGICK = "magick";
    private Paths Paths { get; }

    public Align(IOptions<Paths> paths) => Paths = paths.Value;

    public string Run(string fileName)
    {
      string aligned = Path.Combine(Paths.Aligned, Path.GetFileName(fileName));
      MAGICK.Exec($"{fileName} -deskew 40% -trim {aligned}");
      return aligned;
    }
  }
}
