using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Internal;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace app.Extensions
{
  public static class ServiceCollectionExtensions
  {
    public static IMvcBuilder AddWebApi(this IServiceCollection services)
    {
      var builder = services.AddMvcCore();
      builder.AddAuthorization();
      builder.AddFormatterMappings();
      builder.AddJsonFormatters();
      builder.AddCors();

      return new MvcBuilder(builder.Services, builder.PartManager);
    }
  }
}
