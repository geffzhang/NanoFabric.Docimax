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
              siloHost
                .Configure<ClusterOptions>(options =>
                {
                    //options.ClusterId = appInfo.ClusterId;
                    //options.ServiceId = appInfo.Name;
                    options.ClusterId = "dev";
                    options.ServiceId = "Heroes";
                });

            if (hostingEnv.IsDev)
                siloHost.UseDevelopment();
            if (appInfo.IsDockerized)
                siloHost.UseDockerSwarm();
            else
                siloHost.UseConsulClustering(appInfo);

            return siloHost;
        }

        private static ISiloHostBuilder UseDevelopment(this ISiloHostBuilder siloHost)
        {
            siloHost
                .AddMemoryGrainStorage(OrleansConstants.PubSubStore)
                .ConfigureServices(services =>
                {
                    services.Configure<GrainCollectionOptions>(options => { options.CollectionAge = TimeSpan.FromMinutes(1.5); });
                })
                .Configure<ClusterMembershipOptions>(options => options.ExpectedClusterSize = 1);

            return siloHost;
        }

        private static ISiloHostBuilder UseDevelopmentClustering(this ISiloHostBuilder siloHost)
        {
            var siloAddress = IPAddress.Loopback;
            var siloPort = 11111;
            var gatewayPort = 30000;

            return siloHost
                    .AddMemoryGrainStorage(OrleansConstants.GrainPersistenceStorage)
                    .UseDevelopmentClustering(options => options.PrimarySiloEndpoint = new IPEndPoint(siloAddress, siloPort))
                    .ConfigureEndpoints(siloAddress, siloPort, gatewayPort) //, listenOnAnyHostAddress: true)
                ;
        }

        private static ISiloHostBuilder UseConsulClustering(this ISiloHostBuilder siloHost, IAppInfo appInfo)
        {
            var siloAddress = IPAddress.Loopback;
            var siloPort = 11111;
            var gatewayPort = 30000;

            return siloHost
                      .UseConsulClustering(options => {
                          options.Address = new Uri(appInfo.ConsulEndPoint);
                      })
                      .ConfigureEndpoints(siloAddress, siloPort, gatewayPort);
        }

        private static ISiloHostBuilder UseDockerSwarm(this ISiloHostBuilder siloHost)
        {
            var ips = Dns.GetHostAddressesAsync(Dns.GetHostName()).Result;
            var defaultIp = ips.FirstOrDefault();

            return siloHost
                .ConfigureEndpoints(
                    defaultIp,
                    RandomUtils.GenerateNumber(30001, 30100), // todo: really needed random?
                    RandomUtils.GenerateNumber(20001, 20100), // todo: really needed random?
                    listenOnAnyHostAddress: true
                );
        }

    }
}
