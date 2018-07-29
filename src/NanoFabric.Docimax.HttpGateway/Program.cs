using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NLog.Web;
using Rafty.Infrastructure;
using System.IO;

namespace NanoFabric.Docimax.HttpGateway
{
    public class Program
    {
        private const string defaultAddress = "http://localhost:8000";
        private const string addressKey = "serveraddress";

        public static void Main(string[] args)
        {
            var configurationBuilder = new Microsoft.Extensions.Configuration.ConfigurationBuilder()
           .SetBasePath(Directory.GetCurrentDirectory())
           .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
           .AddJsonFile("ocelot.json", true, false)
           .AddEnvironmentVariables()
           .AddCommandLine(args);
           
            if (args != null)
            {
                configurationBuilder.AddCommandLine(args);
            }
            var hostingconfig = configurationBuilder.Build();
            var url = hostingconfig[addressKey] ?? defaultAddress;
          
            IWebHostBuilder builder = new WebHostBuilder();
            builder.UseKestrel()
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseConfiguration(hostingconfig)
                //.ConfigureAppConfiguration((hostingContext, config) =>
                //{
                //    config.SetBasePath(hostingContext.HostingEnvironment.ContentRootPath);
                //    var env = hostingContext.HostingEnvironment;
                //    //config.AddOcelot();
                //    config.AddEnvironmentVariables();
                //})                
                .ConfigureLogging((hostingContext, logging) =>
                 {
                     logging.AddConfiguration(hostingContext.Configuration.GetSection("Logging"));
                     logging.AddConsole();
                     logging.AddDebug();
                 })
                .UseIISIntegration()
                .UseMetricsWebTracking()
                .UseMetricsEndpoints()
                .UseNLog()
                .UseUrls(url)
                .UseStartup<Startup>();
            var host = builder.Build();
            host.Run();
        }
    }
}
