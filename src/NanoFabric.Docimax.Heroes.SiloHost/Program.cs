using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NanoFabric.Docimax.Core;
using NanoFabric.Docimax.Grains.Heroes;
using NanoFabric.Docimax.Heroes.SiloHost.Infrastructure;
using Orleans;
using Orleans.Hosting;
using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.Loader;
using System.Threading;
using System.Threading.Tasks;
using NLog.Extensions.Logging;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Orleans.Configuration;
using System.Net;

namespace NanoFabric.Docimax.Heroes.SiloHost
{
    class Program
    {
        private static NLog.Logger _log;
        private static ISiloHost _siloHost;
        private static HostingEnvironment _hostingEnv;
        private static Stopwatch _startupStopwatch;
        private static readonly ManualResetEvent SiloStopped = new ManualResetEvent(false);

        public static void Main(string[] args)
        {
            _log = NLog.LogManager.LoadConfiguration("nlog.config").GetCurrentClassLogger();
            RunMainAsync(args).Ignore();
            SiloStopped.WaitOne();
        }

        private static async Task RunMainAsync(string[] args)
        {
            try
            {
                _startupStopwatch = Stopwatch.StartNew();
                _hostingEnv = new HostingEnvironment();
                var shortEnvName = AppInfo.MapEnvironmentName(_hostingEnv.Environment);
                var configBuilder = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddCommandLine(args)
                    .AddJsonFile("appsettings.json")
                    .AddJsonFile("config.json")
                    .AddEnvironmentVariables();

                if (_hostingEnv.IsDockerDev)
                    configBuilder.AddJsonFile("config.dev-docker.json", optional: true);

                var config = configBuilder.Build();

                var appInfo = new AppInfo(config);
                Console.Title = $"Silo - {appInfo.Name}";

                _siloHost = BuildSilo(appInfo,config);
                _log.Info($"Initializing Silo { appInfo.Name} ({appInfo.Version}) [{ _hostingEnv.Environment}]...");

               
                AssemblyLoadContext.Default.Unloading += context =>
                {
                    _log.Info("Assembly unloading...");

                    Task.Run(StopSilo);
                    SiloStopped.WaitOne();

                    _log.Info("Assembly unloaded complete.");
                    // Ensure to flush and stop internal timers/threads before application-exit (Avoid segmentation fault on Linux)
                    NLog.LogManager.Shutdown();
                };

                await StartSilo();
            }
            catch (Exception ex)
            {
                _log.Error(ex, "An error has occurred while initializing or starting silo.");
                NLog.LogManager.Shutdown();
            }
        }

        private static ISiloHost BuildSilo(IAppInfo appInfo,IConfiguration config)
        {
            var builder = new SiloHostBuilder()
                .UseHeroConfiguration(appInfo, _hostingEnv)
                .ConfigureLogging(logging => {
                    logging.AddNLog();
                    logging.AddConsole();
                })
                .ConfigureApplicationParts(parts => parts
                    .AddApplicationPart(typeof(HeroGrain).Assembly).WithReferences()
                )
                .AddStartupTask<WarmupStartupTask>()
                .UseServiceProviderFactory(ConfigureServices)
                .UseDashboard(opt =>
                 {
                     opt.Port = 1010;
                 })              
                .UseSignalR();

            return builder.Build();
        }

        private static async Task StartSilo()
        {
            _log.Info("Silo initialized in {timeTaken:#.##}s. Starting...", _startupStopwatch.Elapsed.TotalSeconds);
            await _siloHost.StartAsync();
            _startupStopwatch.Stop();

            _log.Info("Successfully started Silo in {timeTaken:#.##}s (total).", _startupStopwatch.Elapsed.TotalSeconds);
        }

        private static async Task StopSilo()
        {
            _log.Info("Stopping Silo...");

            try
            {
                await _siloHost.StopAsync();
            }
            catch (Exception ex)
            {
                _log.Error(ex, "Stopping Silo failed...");
            }

            _log.Info("Silo shutdown.");

            SiloStopped.Set();
        }

        private static IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddHeroesGrains();
            services.AddSingleton<ILoggerFactory, LoggerFactory>();
            services.AddSingleton(typeof(ILogger<>), typeof(Logger<>));
            services.AddLogging((builder) => builder.SetMinimumLevel(LogLevel.Trace));

            var serviceProvider = services.BuildServiceProvider();

            var loggerFactory = serviceProvider.GetRequiredService<ILoggerFactory>();

            //configure NLog
            loggerFactory.AddNLog(new NLogProviderOptions { CaptureMessageTemplates = true, CaptureMessageProperties = true });
            NLog.LogManager.LoadConfiguration("nlog.config");
            return serviceProvider;
        }
    }
}