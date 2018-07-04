using Microsoft.Extensions.Configuration;

namespace app.Extensions
{
  public static class ConfigurationExtensions
  {
    public static string Get(this IConfiguration configuration, string key)
    {
      return configuration.GetSection(key).Value;
    }
  }
}
