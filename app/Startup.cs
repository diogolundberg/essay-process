using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Amazon.Runtime;
using Amazon.S3;
using app.Extensions;
using app.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace app
{
  public class Startup
  {
    public Startup(IConfiguration configuration)
    {
      Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    public void ConfigureServices(IServiceCollection services)
    {
      services.AddWebApi();
      var awsOptions = Configuration.GetAWSOptions();
      awsOptions.Credentials = new EnvironmentVariablesAWSCredentials();
      services.AddDefaultAWSOptions(awsOptions);
      services.AddAWSService<IAmazonS3>();
      services.AddScoped<UploadService, UploadService>();
    }

    public void Configure(IApplicationBuilder app, IHostingEnvironment env)
    {
      app.UseMvc();
    }
  }
}
