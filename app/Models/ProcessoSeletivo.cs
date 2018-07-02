using System;

namespace app.Models
{
  public class ProcessoSeletivo
  {
    public ProcessoSeletivo()
    {

    }

    public int IdProcessoSeletivo { get; set; }
    public String Codigo { get; set; }
    public DateTime Data { get; set; }
    public int IdSituacaoProcessoSeletivo { get; set; }
    public int IdEmpresa { get; set; }
    public string Descricao { get; set; }
    public string CaminhoImagens { get; set; }
    public string CaminhoImagensProcessadas { get; set; }
    public string CaminhoImagensRecortadas { get; set; }
    public string CaminhoImagensRejeitadas { get; set; }
    public string CaminhoImagensCorrecao { get; set; }
  }
}
