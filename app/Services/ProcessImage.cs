using app.Extensions;
using app.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.IO;

namespace app.Services
{
  public class ProcessImage
  {
    static Logger console = Logger.GetInstance();
    const string TARGET_TYPE = "TIF";
    const string PROCESSED_PREFIX = "p";
    const string ORIGINAL_PREFIX = "o";
    const string BLACKWHITE_PREFIX = "b";
    const string CROPPED_PREFIX = "c";

    //TODO: Implementar atualização desse programa por argumento passado pela linha de comando, para permitir a execução em Linux
    const string MAGICK_PROGRAM = "magick";

    const int NUMBER_OF_BARCODES = 1;


    ProcessoSeletivo processoSeletivo;

    public ProcessImage(ProcessoSeletivo processoSeletivo)
    {
      this.processoSeletivo = processoSeletivo;
    }

    public void Start(string fileList)
    {
      //Carregar lista de arquivos a serem processados
      using (StreamReader fs = File.OpenText(fileList))
      {
        string line = "";
        while ((line = fs.ReadLine()) != null)
        {
          _Process(line);
        }
      }

    }

    private ConvertedImageInfo _GetConvertedImageInfo(string info)
    {
      int posArrow = info.LastIndexOf("=>");
      int posLeftSpace = info.LastIndexOf(" ", posArrow);
      int posRightSpace = info.IndexOf(" ", posArrow);

      string[] originalSize = info.Substring(posLeftSpace + 1, (posArrow - posLeftSpace) - 1).Split("x");
      string[] resultSize = info.Substring(posArrow + 2, (posRightSpace - posArrow) - 2).Split("x");

      return new ConvertedImageInfo(new Size(Convert.ToInt32(originalSize[0]), Convert.ToInt32(originalSize[1])), new Size(Convert.ToInt32(resultSize[0]), Convert.ToInt32(resultSize[1])));
    }

    public void Process(string fileName)
    {
      this._Process(fileName);
    }

    private bool _Process(string fileName)
    {
      string targetFileName = FileExtensions.RemoveFileExtension(fileName) + "." + TARGET_TYPE;

      string sourceFile = processoSeletivo.CaminhoImagens + "/" + fileName;
      string targetFile = processoSeletivo.CaminhoImagensProcessadas + "/" + targetFileName;

      //Se arquivo não existir, não prossegue com o processamento
      if (!File.Exists(sourceFile))
      {
        console.Warn("Arquivo '" + fileName + "' não encontrado");
        return false;
      }
      FileExtensions.DeleteFile(targetFile);

      char DblQuote = '"';
      //Reduz tamanho da imagem
      FileExtensions.ExecShell(MAGICK_PROGRAM, " convert -resize 1070 " + DblQuote + sourceFile + DblQuote + " " + DblQuote + targetFile + DblQuote, true);

      //Desentorta imagem
      var convertedInfo = _GetConvertedImageInfo(FileExtensions.ExecShell(MAGICK_PROGRAM, DblQuote + targetFile + DblQuote + " -verbose -deskew 40% -trim " + DblQuote + targetFile + DblQuote, true));

      Barcode[] barcode = new Barcode[] { new Barcode("EAN13", fileName.Replace("." + TARGET_TYPE, "")) };
      string blackFile = processoSeletivo.CaminhoImagensProcessadas + "/" + BLACKWHITE_PREFIX + "_" + barcode[0].Code + "." + TARGET_TYPE;
      string processedFile = processoSeletivo.CaminhoImagensProcessadas + "/" + PROCESSED_PREFIX + "_" + barcode[0].Code + "." + TARGET_TYPE;

      FileExtensions.DeleteFile(blackFile);

      //Gera o arquivo em preto e branco
      FileExtensions.ExecShell(MAGICK_PROGRAM, "\"" + targetFile + "\" -level 100,5000,1 -threshold 70% \"" + blackFile + "\"", true);

      //Renomeia o arquivo para o nome do código de barras
      FileExtensions.DeleteFile(processedFile);
      File.Move(targetFile, processedFile);

      Bitmap blackFileImage = (System.Drawing.Bitmap)System.Drawing.Bitmap.FromFile(blackFile);

      string croppedFile = processoSeletivo.CaminhoImagensRecortadas + "/" + CROPPED_PREFIX + "_red_" + barcode[0].Code + "." + TARGET_TYPE;
      FileExtensions.ExecShell(MAGICK_PROGRAM, "convert -crop 980x1040+50+70 \"" + processedFile + "\" \"" + croppedFile + "\"", true);


      return true;
    }

    private void _RejectFile(string targetFile, string logMessage)
    {
      string destination = targetFile.Replace(processoSeletivo.CaminhoImagensProcessadas, processoSeletivo.CaminhoImagensRejeitadas);
      string logFile = destination.Replace("." + TARGET_TYPE, ".log");

      FileExtensions.DeleteFile(destination);
      FileExtensions.DeleteFile(logFile);
      File.Move(targetFile, destination);

      using (StreamWriter file = new StreamWriter(destination.Replace("." + TARGET_TYPE, ".log"), true))
      {
        file.WriteLine("[" + DateTime.Now.ToString(format: Logger.DATETIME_FORMAT) + "] " + logMessage);
      }
    }
  }
}
