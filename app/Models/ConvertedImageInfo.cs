using System.Drawing;

namespace app.Models
{
  public class ConvertedImageInfo
  {
    public ConvertedImageInfo(Size originalSize, Size resultSize)
    {
      this.OriginalSize = originalSize;
      this.ResultSize = resultSize;
    }

    public Size OriginalSize { get; set; }
    public Size ResultSize { get; set; }
  }
}
