using Microsoft.Extensions.DependencyInjection;
using NanoFabric.Docimax.Core;
using NanoFabric.Docimax.Core.Orleans;
using NanoFabric.Docimax.Core.Utils;
using Orleans.Configuration;
using Orleans.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace NanoFabric.Docimax.Heroes.SiloHost.Infrastructure
{
    public static class SiloBuilderExtensions
    {
        public static ISiloHostBuilder UseHeroConfiguration(this ISiloHostBuilder siloHost, IAppInfo appInfo, HostingEnvironment hostingEnv)
        {
            var siloAddress = IPAddress.Loopback;
            var siloPort = 11111;
            var gatewayPort = 30000;

            siloHost
                .Configure<ClusterOptions>(options =>
                {
                    options.ClusterId = "dev";
                    options.ServiceId = "heroes";
                })
                .AddMemoryGrainStorage(OrleansConstants.GrainPersistenceStorage)
                .UseConsulClustering(options => {
                    options.Address = new Uri(appInfo.ConsulEndPoint);
                })
                .ConfigureEndpoints(siloAddress, siloPort, gatewayPort); ;

            return siloHost;
        }

        private static ISiloHostBuilder UseDevelopment(this ISiloHostBuilder siloHost)
        {
            var siloAddress = IPAddress.Loopback;
            var siloPort = 11111;
            var gatewayPort = 30000;

            return siloHost
                    .AddMemoryGrainStorage(OrleansConstants.GrainPersistenceStorage)
                    .UseDevelopmentClustering(options => options.PrimarySiloEndpoint = new IPEndPoint(siloAddress, siloPort))
                    .ConfigureEndpoints(siloAddress, siloPort, gatewayPort);
        }

    }
}
