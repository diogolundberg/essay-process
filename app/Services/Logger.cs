using System;

namespace app.Services
{
  public enum EnumLogLevel
  {
    DEBUG = 1,
    INFO,
    WARN,
    ERROR,
    FATAL
  }

  public class Logger
  {

    public EnumLogLevel level = EnumLogLevel.INFO;

    private static Logger instance;

    public const string DATETIME_FORMAT = "dd/MM/yyyy HH:mm:ss.fffffff";

    private Logger()
    {

    }

    public static Logger GetInstance()
    {
      if (instance == null)
      {
        instance = new Logger();
      }
      return instance;
    }

    public static Logger GetInstance(string level)
    {
      var obj = GetInstance();
      obj.level = GetLevelByName(level);
      return obj;
    }

    private static EnumLogLevel GetLevelByName(string level)
    {
      if (level.Equals(EnumLogLevel.DEBUG.ToString()))
      {
        return EnumLogLevel.DEBUG;
      }
      else if (level.Equals(EnumLogLevel.INFO.ToString()))
      {
        return EnumLogLevel.INFO;
      }
      else if (level.Equals(EnumLogLevel.ERROR.ToString()))
      {
        return EnumLogLevel.ERROR;
      }
      else if (level.Equals(EnumLogLevel.WARN.ToString()))
      {
        return EnumLogLevel.WARN;
      }
      else if (level.Equals(EnumLogLevel.FATAL.ToString()))
      {
        return EnumLogLevel.FATAL;
      }
      return EnumLogLevel.DEBUG;
    }

    private void Log(EnumLogLevel level, string message)
    {
      if (level >= this.level)
      {
        Console.WriteLine("[{0}] {1} - {2}", DateTime.Now.ToString(format: DATETIME_FORMAT), level.ToString(), message);
      }
    }

    public void Debug(string message)
    {
      Log(EnumLogLevel.DEBUG, message);
    }

    public void Info(string message)
    {
      Log(EnumLogLevel.INFO, message);
    }

    public void Warn(string message)
    {
      Log(EnumLogLevel.WARN, message);
    }

    public void Error(string message)
    {
      Log(EnumLogLevel.ERROR, message);
    }

    public void Fatal(string message)
    {
      Log(EnumLogLevel.FATAL, message);
    }

  }
}
