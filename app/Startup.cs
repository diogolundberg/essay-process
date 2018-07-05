using Amazon.Runtime;
using Amazon.S3;
using app.Extensions;
using app.Options;
using app.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace app
{
  public class Startup
  {
    public IConfiguration Configuration { get; }

    public Startup(IConfiguration configuration) => Configuration = configuration;

    public void Configure(IApplicationBuilder app) => app.UseMvc();

    public void ConfigureServices(IServiceCollection services)
    {
      var awsOptions = Configuration.GetAWSOptions();
      awsOptions.Credentials = new EnvironmentVariablesAWSCredentials();
      services.AddDefaultAWSOptions(awsOptions);
      services.AddAWSService<IAmazonS3>();
      services.AddScoped<Download, Download>();
      services.AddScoped<Upload, Upload>();
      services.AddScoped<Process, Process>();
      services.AddWebApi();
      services.Configure<Paths>(Configuration.GetSection("ImagesPath"));
    }
  }
}
