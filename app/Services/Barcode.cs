namespace app.Services
{
  public class Barcode
  {
    public Barcode(string type, string code)
    {
      this.Type = type;
      this.Code = code;
    }

    public override string ToString()
    {
      return Type + ":" + Code;
    }

    public string Type { get; set; }
    public string Code { get; set; }
  }
}
